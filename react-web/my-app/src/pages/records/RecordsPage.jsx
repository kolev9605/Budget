import React, { useState, useEffect } from "react"; // Add React to the import statement
import { NavLink } from "react-router";
import {
  PlusIcon,
  ChevronDownIcon,
  PencilIcon,
  TrashIcon,
  MagnifyingGlassIcon,
  DocumentTextIcon,
  ChevronLeftIcon,
  ChevronRightIcon,
} from "@heroicons/react/24/outline";
import { getRecords } from "../../api/records.service.js"; // Import the getRecords function
import axiosInstance from "../../api/createAxiosAuth.js";
import LoadingOverlay from "../../components/LoadingOverlay.jsx";

const RecordsPage = () => {
  const [records, setRecords] = useState([]);
  const [groupedRecords, setGroupedRecords] = useState({});
  const [filteredRecords, setFilteredRecords] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [filterType, setFilterType] = useState("all");
  const [selectedAccount, setSelectedAccount] = useState("all");
  const [dateRange, setDateRange] = useState("all");
  const [specificMonth, setSpecificMonth] = useState(new Date()); // Use Date object for specific month
  const [showFilters, setShowFilters] = useState(false); // New state for toggling filters
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchRecords = async () => {
      try {
        const response = await getRecords(axiosInstance);
        console.log("response.items", response.items);

        setRecords(response.items);
        setFilteredRecords(response.items);

        const groupedRecords = response.items.reduce((groups, record) => {
          const date = new Date(record.recordDate).toLocaleDateString("en-US", {
            year: "numeric",
            month: "short",
            day: "numeric",
          });
          if (!groups[date]) {
            groups[date] = [];
          }
          groups[date].push(record);
          return groups;
        }, {});

        setGroupedRecords(groupedRecords);

        console.log(groupedRecords);
        console.log(
          "hii iiiii",
          Object.entries(groupedRecords).map(([date, records]) => ({ date, records }))
        );
        console.log(filteredRecords);
        console.log(records);
      } finally {
        setIsLoading(false);
      }
    };

    fetchRecords();
  }, []);

  const formatMonth = (date) => date.toLocaleDateString("en-US", { year: "numeric", month: "long" });

  const handlePrevMonth = () => {
    setSpecificMonth(new Date(specificMonth.getFullYear(), specificMonth.getMonth() - 1, 1));
  };

  const handleNextMonth = () => {
    setSpecificMonth(new Date(specificMonth.getFullYear(), specificMonth.getMonth() + 1, 1));
  };

  const sampleAccounts = [
    { id: "1", name: "Cash", type: "cash", balance: 2450.75, description: "Physical cash and coins" },
    { id: "2", name: "Primary Credit Card", type: "credit", balance: -1250.0, description: "Visa Platinum **** 1234" },
    { id: "3", name: "Savings Account", type: "savings", balance: 15000.0, description: "Bank of Example - 5% APY" },
  ];

  const getDateRange = () => {
    const now = new Date();
    switch (dateRange) {
      case "today":
        return { start: new Date(now.setHours(0, 0, 0, 0)), end: new Date(now.setHours(23, 59, 59, 999)) };
      case "week":
        const startOfWeek = new Date(now.setDate(now.getDate() - now.getDay()));
        return { start: new Date(startOfWeek.setHours(0, 0, 0, 0)), end: new Date() };
      case "month":
        return {
          start: new Date(now.getFullYear(), now.getMonth(), 1),
          end: new Date(now.getFullYear(), now.getMonth() + 1, 0),
        };
      case "year":
        return {
          start: new Date(now.getFullYear(), 0, 1),
          end: new Date(now.getFullYear(), 11, 31),
        };
      default:
        return { start: null, end: null };
    }
  };

  return isLoading ? (
    <LoadingOverlay />
  ) : (
    <div className="min-h-screen bg-gray-900 p-6 sm:p-8 lg:p-10">
      <div className="max-w-7xl mx-auto">
        {/* Header */}
        <div className="flex items-center justify-between mb-6 gap-3 flex-wrap">
          <h1 className="text-xl sm:text-2xl font-bold text-gray-100 flex items-center gap-2">
            <DocumentTextIcon className="h-5 sm:h-6 w-5 sm:w-6 text-blue-400" />
            Records
          </h1>
          <NavLink
            to="/records/new"
            className="bg-blue-500 hover:bg-blue-400 text-white px-4 py-2 sm:px-6 sm:py-3 rounded-lg sm:rounded-xl
              flex items-center gap-2 transition-colors text-sm sm:text-base whitespace-nowrap"
          >
            <PlusIcon className="h-4 sm:h-5 w-4 sm:w-5" />
            Add Record
          </NavLink>
        </div>

        {/* Filters */}
        <div className="bg-gray-800 rounded-xl p-4 mb-6 shadow-lg">
          <button
            onClick={() => setShowFilters(!showFilters)}
            className="md:hidden bg-gray-700 text-gray-100 px-4 py-2 rounded-lg w-full text-left flex justify-between items-center"
          >
            <span>Filters</span>
            <ChevronDownIcon className={`h-5 w-5 transition-transform ${showFilters ? "rotate-180" : "rotate-0"}`} />
          </button>
          <div
            className={`grid grid-cols-1 md:grid-cols-4 gap-3 mt-4 md:mt-0 ${showFilters ? "block" : "hidden md:grid"}`}
          >
            <div className="relative">
              <input
                type="text"
                placeholder="Search..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="w-full bg-gray-700 border border-gray-600 rounded-lg pl-10 pr-3 py-2
                  text-gray-100 placeholder-gray-500 focus:outline-none focus:border-blue-400
                  focus:ring-1 focus:ring-blue-400/30 text-sm"
              />
              <MagnifyingGlassIcon className="h-4 w-4 text-gray-400 absolute left-3 top-1/2 -translate-y-1/2" />
            </div>

            <select
              value={filterType}
              onChange={(e) => setFilterType(e.target.value)}
              className="bg-gray-700 border border-gray-600 rounded-lg px-3 py-2
                text-gray-100 focus:outline-none focus:border-blue-400 focus:ring-1 
                focus:ring-blue-400/30 text-sm"
            >
              <option value="all">All Types</option>
              <option value="income">Income</option>
              <option value="expense">Expense</option>
              <option value="transfer">Transfer</option>
            </select>

            <select
              value={selectedAccount}
              onChange={(e) => setSelectedAccount(e.target.value)}
              className="bg-gray-700 border border-gray-600 rounded-lg px-3 py-2
                text-gray-100 focus:outline-none focus:border-blue-400 focus:ring-1 
                focus:ring-blue-400/30 text-sm"
            >
              <option value="all">All Accounts</option>
              {sampleAccounts.map((account) => (
                <option key={account.id} value={account.id}>
                  {account.name}
                </option>
              ))}
            </select>

            <select
              value={dateRange}
              onChange={(e) => setDateRange(e.target.value)}
              className="bg-gray-700 border border-gray-600 rounded-lg px-3 py-2
                text-gray-100 focus:outline-none focus:border-blue-400 focus:ring-1 
                focus:ring-blue-400/30 text-sm"
            >
              <option value="all">All Time</option>
              <option value="today">Today</option>
              <option value="week">This Week</option>
              <option value="month">This Month</option>
              <option value="year">This Year</option>
              <option value="custom">Custom</option>
            </select>
          </div>

          {dateRange === "custom" && (
            <div className={`flex items-center justify-center mt-4 ${showFilters ? "block" : "hidden"} md:flex`}>
              <button onClick={handlePrevMonth} className="hover:text-blue-400">
                <ChevronLeftIcon className="h-5 w-5 text-gray-400" />
              </button>
              <span className="mx-4 text-gray-100 text-sm">{formatMonth(specificMonth)}</span>
              <button onClick={handleNextMonth} className="hover:text-blue-400">
                <ChevronRightIcon className="h-5 w-5 text-gray-400" />
              </button>
            </div>
          )}
        </div>

        {/* Records Table (Desktop) */}
        <div className="hidden md:block bg-gray-800 rounded-xl shadow-lg overflow-hidden">
          <table className="min-w-full table-auto text-sm text-left text-gray-400">
            <thead className="bg-gray-700 text-gray-300 uppercase text-xs font-medium">
              <tr>
                <th className="px-7 py-3">Date</th>
                <th className="px-7 py-3">Description</th>
                <th className="px-7 py-3">Account</th>
                <th className="px-7 py-3">Category</th>
                <th className="px-7 py-3 text-right">Amount</th>
                <th className="px-7 py-3 text-right">Actions</th>
              </tr>
            </thead>
            <tbody>
              {Object.entries(groupedRecords).map(([date, records], index) => (
                <React.Fragment key={index}>
                  <tr className="bg-gray-700">
                    <td colSpan="6" className="px-7 py-3 text-gray-300 font-semibold">
                      {date}
                    </td>
                  </tr>
                  {records.map((record) => (
                    <tr key={record.id} className="border-b border-gray-700 hover:bg-gray-750 transition-colors">
                      <td className="px-7 py-4 text-gray-400">
                        {new Date(record.recordDate).toLocaleTimeString("en-US", {
                          hour: "2-digit",
                          minute: "2-digit",
                        })}
                      </td>
                      <td className="px-7 py-3 text-gray-100 truncate">{record.note}</td>
                      <td className="px-7 py-3 text-gray-500">
                        {record.recordType === "Transfer" ? (
                          <>
                            {record.account.name} → {record.fromAccount.name}
                          </>
                        ) : (
                          record.account.name
                        )}
                      </td>
                      <td className="px-7 py-3 text-gray-500">{record.category.name || "-"}</td>
                      <td
                        className={`px-7 py-3 text-right font-medium ${
                          record.recordType === "Income"
                            ? "text-green-400"
                            : record.recordType === "Expense"
                            ? "text-red-400"
                            : "text-blue-400"
                        }`}
                      >
                        {record.recordType !== "Transfer" && (
                          <span className="text-xs">{record.recordType === "Income" ? "+" : "-"}</span>
                        )}
                        $
                        {Math.abs(record.amount).toLocaleString(undefined, {
                          minimumFractionDigits: 2,
                          maximumFractionDigits: 2,
                        })}
                      </td>
                      <td className="px-7 py-3 text-right">
                        <div className="flex items-center justify-end gap-2">
                          <NavLink
                            to={`/records/edit/${record.id}`}
                            className="text-gray-400 hover:text-blue-400 p-1 rounded-lg transition-colors"
                          >
                            <PencilIcon className="h-4 w-4" />
                          </NavLink>
                          <button className="text-gray-400 hover:text-red-400 p-1 rounded-lg transition-colors">
                            <TrashIcon className="h-4 w-4" />
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </React.Fragment>
              ))}

              {filteredRecords.length === 0 && (
                <tr>
                  <td colSpan="6" className="px-7 py-3 text-center text-gray-400 text-sm">
                    No transactions found matching your criteria
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>

        {/* Records Cards (Mobile) */}
        <div className="block md:hidden space-y-4">
          {Object.entries(groupedRecords).map(([date, records], index) => (
            <div key={index} className="bg-gray-700 rounded-lg shadow-md">
              <div className="px-4 py-2 text-gray-300 font-semibold">{date}</div>
              {records.map((record, index) => (
                <div key={index} className="bg-gray-800 p-2 hover:bg-gray-750 transition-colors">
                  <div className="flex justify-between items-center">
                    <div className="text-gray-400 text-xs">
                      {new Date(record.recordDate).toLocaleTimeString("en-US", {
                        hour: "2-digit",
                        minute: "2-digit",
                      })}
                    </div>
                    <div
                      className={`font-medium text-sm ${
                        record.recordType === "Income"
                          ? "text-green-400"
                          : record.recordType === "Expense"
                          ? "text-red-400"
                          : "text-blue-400"
                      }`}
                    >
                      {record.recordType !== "Transfer" && (
                        <span className="text-xs">{record.recordType === "Income" ? "+" : "-"}</span>
                      )}
                      $
                      {Math.abs(record.amount).toLocaleString(undefined, {
                        minimumFractionDigits: 2,
                        maximumFractionDigits: 2,
                      })}
                    </div>
                  </div>
                  <div className="text-gray-100 text-xs truncate mt-1">{record.note || "No description"}</div>
                  <div className="text-gray-500 text-xs mt-1 flex justify-between">
                    <span>
                      {record.recordType === "Transfer" ? (
                        <>
                          {record.account.name} → {record.fromAccount.name}
                        </>
                      ) : (
                        record.account.name
                      )}
                    </span>
                    {record.category && <span>{record.category.name}</span>}
                  </div>
                </div>
              ))}
            </div>
          ))}

          {filteredRecords.length === 0 && (
            <div className="bg-gray-800 p-4 rounded-lg text-center text-gray-400 text-sm">
              No transactions found matching your criteria
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default RecordsPage;
