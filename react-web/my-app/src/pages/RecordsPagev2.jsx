import { useState } from "react";
import { Link } from "react-router-dom";
import {
  PlusIcon,
  FunnelIcon,
  CalendarIcon,
  ChevronDownIcon,
  PencilIcon,
  TrashIcon,
  MagnifyingGlassIcon,
  ArrowsRightLeftIcon,
  BanknotesIcon,
} from "@heroicons/react/24/outline";

const RecordsPagev2 = ({ records, accounts }) => {
  const [searchTerm, setSearchTerm] = useState("");
  const [filterType, setFilterType] = useState("all");
  const [selectedAccount, setSelectedAccount] = useState("all");
  const [dateRange, setDateRange] = useState("all");
  const [isFilterOpen, setIsFilterOpen] = useState(false);

  // Sample data - replace with real data

  const sampleAccounts = [
    { id: "1", name: "Cash", type: "cash", balance: 2450.75, description: "Physical cash and coins" },
    { id: "2", name: "Primary Credit Card", type: "credit", balance: -1250.0, description: "Visa Platinum **** 1234" },
    { id: "3", name: "Savings Account", type: "savings", balance: 15000.0, description: "Bank of Example - 5% APY" },
  ];

  const sampleRecords = [
    {
      id: 1,
      date: "2024-03-15",
      amount: -245.75,
      category: "Food",
      type: "expense",
      account: "Cash",
      note: "Grocery shopping",
      destinationAccount: "",
    },
    {
      id: 2,
      date: "2024-03-14",
      amount: 5000.0,
      category: "Salary",
      type: "income",
      account: "Bank Account",
      note: "Monthly salary",
      destinationAccount: "",
    },
    {
      id: 3,
      date: "2024-03-13",
      amount: -1000.0,
      type: "transfer",
      account: "Savings",
      note: "Monthly savings",
      destinationAccount: "Investment Account",
    },
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

  const filteredRecords = sampleRecords.filter((record) => {
    const matchesSearch = record.note.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesType = filterType === "all" || record.type === filterType;
    const matchesAccount =
      selectedAccount === "all" || record.account === selectedAccount || record.destinationAccount === selectedAccount;

    const { start, end } = getDateRange();
    const matchesDate = !start || (new Date(record.date) >= start && new Date(record.date) <= end);

    return matchesSearch && matchesType && matchesAccount && matchesDate;
  });

  return (
    <div className="min-h-screen bg-gray-900 p-4 sm:p-6 lg:p-8">
      <div className="max-w-7xl mx-auto">
        {/* Header */}
        <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between mb-6 gap-3">
          <h1 className="text-xl font-bold text-gray-100">Transactions</h1>
          <Link
            to="/records/new"
            className="bg-blue-500 hover:bg-blue-400 text-white px-4 py-2 rounded-xl
              flex items-center gap-2 transition-colors text-sm"
          >
            <PlusIcon className="h-4 w-4" />
            New
          </Link>
        </div>

        {/* Filters */}
        <div className="bg-gray-800 rounded-xl p-4 mb-6 shadow-lg">
          <div className="grid grid-cols-1 md:grid-cols-5 gap-3">
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
            </select>

            <button
              onClick={() => setIsFilterOpen(!isFilterOpen)}
              className="bg-gray-700 border border-gray-600 rounded-lg px-3 py-2
                text-gray-100 hover:bg-gray-600 transition-colors text-sm flex items-center justify-center gap-1"
            >
              <FunnelIcon className="h-4 w-4" />
              More
            </button>
          </div>
        </div>

        {/* Records List */}
        <div className="space-y-2">
          {filteredRecords.map((record) => (
            <div
              key={record.id}
              className="bg-gray-800 p-4 rounded-xl shadow-lg hover:bg-gray-750 transition-colors group"
            >
              <div className="grid grid-cols-5 items-center gap-4 text-sm">
                {/* Date */}
                <div className="text-gray-400">
                  {new Date(record.date).toLocaleDateString("en-US", {
                    month: "short",
                    day: "numeric",
                  })}
                </div>

                {/* Description */}
                <div className="col-span-2">
                  <div className="text-gray-100 truncate">{record.note}</div>
                  <div className="text-gray-500 text-xs flex items-center gap-1 mt-1">
                    {record.type === "transfer" ? (
                      <>
                        <ArrowsRightLeftIcon className="h-3 w-3" />
                        <span className="truncate">
                          {record.account} → {record.destinationAccount}
                        </span>
                      </>
                    ) : (
                      <>
                        <BanknotesIcon className="h-3 w-3" />
                        <span className="truncate">{record.account}</span>
                        {record.category && <span className="mx-1">•</span>}
                        <span className="truncate">{record.category}</span>
                      </>
                    )}
                  </div>
                </div>

                {/* Amount */}
                <div
                  className={`text-right font-medium ${
                    record.type === "income"
                      ? "text-green-400"
                      : record.type === "expense"
                      ? "text-red-400"
                      : "text-blue-400"
                  }`}
                >
                  {record.type !== "transfer" && (
                    <span className="text-xs">{record.type === "income" ? "+" : "-"}</span>
                  )}
                  $
                  {Math.abs(record.amount).toLocaleString(undefined, {
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2,
                  })}
                </div>

                {/* Actions */}
                <div className="flex items-center justify-end gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
                  <Link
                    to={`/records/edit/${record.id}`}
                    className="text-gray-400 hover:text-blue-400 p-1 rounded-lg transition-colors"
                  >
                    <PencilIcon className="h-4 w-4" />
                  </Link>
                  <button className="text-gray-400 hover:text-red-400 p-1 rounded-lg transition-colors">
                    <TrashIcon className="h-4 w-4" />
                  </button>
                </div>
              </div>
            </div>
          ))}

          {filteredRecords.length === 0 && (
            <div className="bg-gray-800 p-6 rounded-xl text-center text-gray-400 text-sm">
              No transactions found matching your criteria
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default RecordsPagev2;
