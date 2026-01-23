import React, { useState, useEffect } from "react";
import { NavLink } from "react-router";
import { PlusIcon, ChevronDownIcon, PencilIcon, TrashIcon, DocumentTextIcon } from "@heroicons/react/24/outline";
import { getRecords, getRecordTypes, deleteRecord } from "../../api/records.service.js";
import LoadingOverlay from "../../components/LoadingOverlay.jsx";
import { toast } from "react-toastify";
import { importWalletRecords } from "../../api/import.service.js";
import { getAccounts } from "../../api/accounts.service.js";
import { getCategories } from "../../api/categories.service.js";
import InfiniteScroll from "react-infinite-scroll-component";
import { getCurrencyLabel } from "../../utils/currencyUtils.js";
import { DateTime } from "luxon";
import PeriodPicker from "../../components/PeriodPicker";

const RecordsPage = () => {
  const [recordTypes, setRecordTypes] = useState([]);
  const [categories, setCategories] = useState([]);
  const [accounts, setAccounts] = useState([]);
  const [groupedRecords, setGroupedRecords] = useState({});
  const [hasNextPage, setHasNextPage] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [selectedRecordType, setSelectedRecordType] = useState("all");
  const [selectedAccount, setSelectedAccount] = useState("all");
  const [selectedDateRange, setSelectedDateRange] = useState("month");
  const [selectedCategory, setSelectedCategory] = useState("all");
  const [referenceDate, setReferenceDate] = useState(new Date());
  const [showFilters, setShowFilters] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const fetchRecords = async (page) => {
    let startOfRange = DateTime.fromJSDate(new Date(referenceDate));
    let endOfRange = DateTime.fromJSDate(new Date(referenceDate));

    switch (selectedDateRange) {
      case "day":
        startOfRange = startOfRange.startOf("day");
        endOfRange = endOfRange.endOf("day");
        break;
      case "week":
        startOfRange = startOfRange.startOf("week");
        endOfRange = endOfRange.endOf("week");
        break;
      case "month":
        startOfRange = startOfRange.startOf("month");
        endOfRange = endOfRange.endOf("month");
        break;
      case "year":
        startOfRange = startOfRange.startOf("year");
        endOfRange = endOfRange.endOf("year");
        break;
      default:
        toast.error("Invalid date range selected.");
        return;
    }

    const response = await getRecords(
      page,
      20,
      selectedAccount,
      startOfRange.toISO(),
      endOfRange.toISO(),
      selectedRecordType,
      selectedCategory
    );

    const newGroupedRecords = response.items.reduce((groups, record) => {
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

    setGroupedRecords((prev) => ({ ...prev, ...newGroupedRecords }));
    setHasNextPage(response.hasNextPage);
  };

  useEffect(() => {
    const fetchInitialData = async () => {
      try {
        setIsLoading(true);
        const accountsResponse = await getAccounts();
        const recordTypesResponse = await getRecordTypes();
        const categoriesResponse = await getCategories(false);

        setAccounts(accountsResponse);
        setRecordTypes(recordTypesResponse);
        setCategories(categoriesResponse);
      } finally {
        setIsLoading(false);
      }
    };

    fetchInitialData();
  }, []);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setIsLoading(true);
        setGroupedRecords({});
        await fetchRecords(1);
      } finally {
        setIsLoading(false);
      }
    };

    fetchData();
  }, [selectedRecordType, selectedAccount, selectedDateRange, selectedCategory, referenceDate]);

  const fetchNextPage = async () => {
    const nextPage = currentPage + 1;
    await fetchRecords(nextPage);
    setCurrentPage(nextPage);
  };

  const handleImport = async (event) => {
    const file = event.target.files[0];
    if (!file) return;

    try {
      setIsLoading(true);
      await importWalletRecords(file);
      toast.success("Records imported successfully!");
    } finally {
      setIsLoading(false);
    }
  };

  // Add delete handler
  const handleDeleteRecord = async (id) => {
    if (!window.confirm("Are you sure you want to delete this record?")) return;
    try {
      setIsLoading(true);
      await deleteRecord(id);
      toast.success("Record deleted successfully!");
      // Remove the deleted record from groupedRecords
      setGroupedRecords((prev) => {
        const updated = {};
        for (const [date, records] of Object.entries(prev)) {
          const filtered = records.filter((r) => r.id !== id);
          if (filtered.length > 0) updated[date] = filtered;
        }
        return updated;
      });
    } finally {
      setIsLoading(false);
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
          <div className="flex gap-3">
            <NavLink
              to="/records/new"
              className="bg-blue-500 hover:bg-blue-400 text-white px-4 py-2 sm:px-6 sm:py-3 rounded-lg sm:rounded-xl
                flex items-center gap-2 transition-colors text-sm sm:text-base whitespace-nowrap"
            >
              <PlusIcon className="h-4 sm:h-5 w-4 sm:w-5" />
              Add Record
            </NavLink>
            <label
              htmlFor="import-file"
              className="bg-green-500 hover:bg-green-400 text-white px-4 py-2 sm:px-6 sm:py-3 rounded-lg sm:rounded-xl
                flex items-center gap-2 transition-colors text-sm sm:text-base cursor-pointer whitespace-nowrap"
            >
              <DocumentTextIcon className="h-4 sm:h-5 w-4 sm:w-5" />
              Import from Wallet
            </label>
            <input id="import-file" type="file" accept=".csv" className="hidden" onChange={handleImport} />
          </div>
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
            <select
              value={selectedRecordType}
              onChange={(e) => setSelectedRecordType(e.target.value)}
              className="bg-gray-700 border border-gray-600 rounded-lg px-3 py-2
                text-gray-100 focus:outline-none focus:border-blue-400 focus:ring-1 
                focus:ring-blue-400/30 text-sm"
            >
              <option value="all">All Types</option>
              {recordTypes.map((rt, index) => (
                <option key={index} value={rt}>
                  {rt}
                </option>
              ))}
            </select>

            <select
              value={selectedAccount}
              onChange={(e) => setSelectedAccount(e.target.value)}
              className="bg-gray-700 border border-gray-600 rounded-lg px-3 py-2
                text-gray-100 focus:outline-none focus:border-blue-400 focus:ring-1 
                focus:ring-blue-400/30 text-sm"
            >
              <option value="all">All Accounts</option>
              {accounts.map((account) => (
                <option key={account.id} value={account.id}>
                  {account.name}
                </option>
              ))}
            </select>

            <select
              value={selectedCategory}
              onChange={(e) => setSelectedCategory(e.target.value)}
              className="bg-gray-700 border border-gray-600 rounded-lg px-3 py-2
                text-gray-100 focus:outline-none focus:border-blue-400 focus:ring-1 
                focus:ring-blue-400/30 text-sm"
            >
              <option value="all">All Categories</option>
              {categories.map((ct, index) => (
                <option key={index} value={ct.id}>
                  {ct.name}
                </option>
              ))}
            </select>

            <select
              value={selectedDateRange}
              onChange={(e) => setSelectedDateRange(e.target.value)}
              className="bg-gray-700 border border-gray-600 rounded-lg px-3 py-2
                text-gray-100 focus:outline-none focus:border-blue-400 focus:ring-1 
                focus:ring-blue-400/30 text-sm"
            >
              <option value="day">Day</option>
              <option value="week">Week</option>
              <option value="month">Month</option>
              <option value="year">Year</option>
            </select>
          </div>

          <div className="mt-3">
            <PeriodPicker selectedDateRange={selectedDateRange} referenceDate={referenceDate} setReferenceDate={setReferenceDate}/>
          </div>
        </div>

        {/* Records Table (Desktop) */}
        <div className="hidden md:block bg-gray-800 rounded-xl shadow-lg overflow-hidden">
          <InfiniteScroll
            dataLength={Object.keys(groupedRecords).length}
            next={fetchNextPage}
            hasMore={hasNextPage}
            loader={<div className="text-center text-gray-400 py-4">Loading more records...</div>}
          >
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
                          {getCurrencyLabel(record.account.currency.abbreviation, record.amount)}
                        </td>
                        <td className="px-7 py-3 text-right">
                          <div className="flex items-center justify-end gap-2">
                            <NavLink
                              to={`/records/edit/${record.id}`}
                              className="text-gray-400 hover:text-blue-400 p-1 rounded-lg transition-colors"
                            >
                              <PencilIcon className="h-4 w-4" />
                            </NavLink>
                            <button
                              className="text-gray-400 hover:text-red-400 p-1 rounded-lg transition-colors"
                              onClick={() => handleDeleteRecord(record.id)}
                            >
                              <TrashIcon className="h-4 w-4" />
                            </button>
                          </div>
                        </td>
                      </tr>
                    ))}
                  </React.Fragment>
                ))}

                {Object.keys(groupedRecords).length === 0 && (
                  <tr>
                    <td colSpan="6" className="px-7 py-3 text-center text-gray-400 text-sm">
                      No records found matching your criteria
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </InfiniteScroll>
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
                      {getCurrencyLabel(record.account.currency.abbreviation, record.amount)}
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

          {Object.keys(groupedRecords).length === 0 && (
            <div className="bg-gray-800 p-4 rounded-lg text-center text-gray-400 text-sm">
              No records found matching your criteria
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default RecordsPage;
