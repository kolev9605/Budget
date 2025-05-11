import { useState, useEffect } from "react";
import {
  ArrowUpIcon,
  ArrowDownIcon,
  CurrencyDollarIcon,
  ChartBarIcon,
  PlusIcon,
  SparklesIcon,
} from "@heroicons/react/24/outline";
import { XAxis, YAxis, Tooltip, ResponsiveContainer, LineChart, Line, CartesianGrid, Legend } from "recharts";
import PeriodPicker from "../components/PeriodPicker";
import { DateTime } from "luxon";
import { getStatistics, getTotalBalance, askAiGenerate, createRecord } from "../api/records.service";
import LoadingOverlay from "../components/LoadingOverlay";
import Modal from "../components/Modal";
import { getCurrencyLabel } from "../utils/currencyUtils";
import { Link } from "react-router-dom";

const DashboardPage = () => {
  const [quickAddText, setQuickAddText] = useState("");
  const [selectedDateRange, setSelectedDateRange] = useState("");
  const [referenceDate, setReferenceDate] = useState(new Date());
  const [isLoading, setIsLoading] = useState(true);
  const [dashboardData, setDashboardData] = useState(null);
  const [recordsData, setRecordsData] = useState(null);
  const [chartData, setChartData] = useState([]);
  const [aiGeneratedRecord, setAiGeneratedRecord] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);

  useEffect(() => {
    setSelectedDateRange("month");
    let startOfRange = DateTime.fromJSDate(new Date(referenceDate)).startOf("month");
    let endOfRange = DateTime.fromJSDate(new Date(referenceDate)).endOf("month");

    const fetchDashboardData = async () => {
      try {
        const recordsResponse = await getStatistics(startOfRange.toISO(), endOfRange.toISO());
        const totalBalanceResponse = await getTotalBalance();

        const totalExpenses = recordsResponse
          .filter((r) => r.amount < 0 && r.recordType === "Expense")
          .reduce((acc, item) => acc + item.amount, 0);

        const totalIncome = recordsResponse
          .filter((r) => r.amount > 0 && r.recordType === "Income")
          .reduce((acc, item) => acc + item.amount, 0);

        setRecordsData(recordsResponse);
        setDashboardData({
          totalBalance: totalBalanceResponse,
          income: totalIncome,
          expense: Math.abs(totalExpenses),
          savingRate: totalIncome ? ((totalIncome + totalExpenses) / totalIncome) * 100 : 0,
        });
      } finally {
        setIsLoading(false);
      }
    };

    fetchDashboardData();
  }, [referenceDate]);

  useEffect(() => {
    if (recordsData) {
      const groupedData = Object.entries(
        recordsData.reduce((acc, record) => {
          const dateKey = DateTime.fromISO(record.recordDate).toISODate();
          acc[dateKey] = (acc[dateKey] || 0) + record.amount;
          return acc;
        }, {})
      )
        .map(([date, amount]) => ({ recordDate: DateTime.fromISO(date).toFormat("MMM dd"), cashFlow: amount }))
        .sort((a, b) => DateTime.fromFormat(a.recordDate, "MMM dd") - DateTime.fromFormat(b.recordDate, "MMM dd"));

      let cumulativeSum = 0;
      const cumulativeData = groupedData.map((item) => {
        cumulativeSum += item.cashFlow;
        return { ...item, cashFlow: Math.round(cumulativeSum * 100) / 100 };
      });

      setChartData(cumulativeData);
    }
  }, [recordsData]);

  const handleQuickAdd = async () => {
    if (!quickAddText.trim()) return; // Ensure input is not empty
    try {
      const response = await askAiGenerate(quickAddText); // Use free text input
      setAiGeneratedRecord(response || null);
      setIsModalOpen(true);
    } catch (error) {
      console.error("Error generating AI record:", error);
      setAiGeneratedRecord(null); // Handle error case
      setIsModalOpen(true);
    }
  };

  const handleConfirmRecord = async () => {
    if (!aiGeneratedRecord) return;
    try {
      await createRecord(aiGeneratedRecord);
      setIsModalOpen(false);
      setQuickAddText("");
    } catch (error) {
      console.error("Error creating record:", error);
    }
  };

  // Helper to get top 4 expense categories from recordsData
  const getTopExpenseCategories = () => {
    if (!recordsData) return [];
    // Group by categoryName and sum negative amounts (expenses), exclude transfers
    const categoryTotals = recordsData.reduce((acc, record) => {
      if (record.amount < 0 && record.recordType !== "Transfer") {
        acc[record.categoryName] = (acc[record.categoryName] || 0) + Math.abs(record.amount);
      }
      return acc;
    }, {});
    // Convert to array and sort by total descending
    const sorted = Object.entries(categoryTotals)
      .map(([name, amount]) => ({ name, amount }))
      .sort((a, b) => b.amount - a.amount)
      .slice(0, 4);
    // Assign a color for each (fallback to blue if >4)
    const colors = ["bg-blue-400", "bg-purple-400", "bg-green-400", "bg-yellow-400"];
    return sorted.map((cat, idx) => ({ ...cat, color: colors[idx] || "bg-blue-400" }));
  };

  return isLoading ? (
    <LoadingOverlay />
  ) : (
    <div className="min-h-screen bg-gray-900">
      {/* Navbar - Use the previous navbar component */}

      <main className="p-6 sm:p-8 lg:p-10 max-w-7xl mx-auto">
        <div className="bg-gray-800 rounded-xl p-4 mb-6 shadow-lg">
          <PeriodPicker
            selectedDateRange={selectedDateRange}
            referenceDate={referenceDate}
            setReferenceDate={setReferenceDate}
          />
        </div>
        {/* Quick Stats Row */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
          <div className="bg-gray-800 p-6 rounded-2xl shadow-lg">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-400 text-sm mb-1">Total Balance</p>
                <p className="text-2xl font-bold text-gray-100">
                  {getCurrencyLabel("BGN", dashboardData.totalBalance)}
                </p>
              </div>
              <CurrencyDollarIcon className="h-8 w-8 text-blue-400" />
            </div>
          </div>

          <div className="bg-gray-800 p-6 rounded-2xl shadow-lg">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-400 text-sm mb-1">Total Income</p>
                <p className="text-2xl font-bold text-green-400">+{getCurrencyLabel("BGN", dashboardData.income)}</p>
              </div>
              <ArrowUpIcon className="h-8 w-8 text-green-400" />
            </div>
          </div>

          <div className="bg-gray-800 p-6 rounded-2xl shadow-lg">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-400 text-sm mb-1">Total Expenses</p>
                <p className="text-2xl font-bold text-red-400">-{getCurrencyLabel("BGN", dashboardData.expense)}</p>
              </div>
              <ArrowDownIcon className="h-8 w-8 text-red-400" />
            </div>
          </div>

          <div className="bg-gray-800 p-6 rounded-2xl shadow-lg">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-400 text-sm mb-1">Savings Rate</p>
                <p className="text-2xl font-bold text-blue-400">{dashboardData.savingRate.toFixed(2)}%</p>
              </div>
              <ChartBarIcon className="h-8 w-8 text-blue-400" />
            </div>
          </div>
        </div>

        {/* Main Content */}
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* Chart Section */}
          <div className="lg:col-span-2 bg-gray-800 p-6 rounded-2xl shadow-lg">
            <h3 className="text-lg font-semibold text-gray-100 mb-6">Cash Flow Comparison</h3>
            <div className="h-80">
              <ResponsiveContainer width="100%" height="100%">
                <LineChart data={chartData}>
                  <CartesianGrid stroke="#374151" strokeDasharray="3 3" />
                  <XAxis dataKey="recordDate" stroke="#6B7280" />
                  <YAxis stroke="#6B7280" />
                  <Tooltip
                    contentStyle={{ backgroundColor: "#1F2937", border: "none" }}
                    labelStyle={{ color: "#F9FAFB" }}
                    itemStyle={{ color: "#F9FAFB" }}
                  />
                  <Legend wrapperStyle={{ color: "#F9FAFB" }} iconType="circle" />
                  <Line type="monotone" dataKey="cashFlow" stroke="#60A5FA" strokeWidth={2} name="Cash Flow" />
                </LineChart>
              </ResponsiveContainer>
            </div>
          </div>

          {/* Recent Transactions */}
          <div className="bg-gray-800 p-6 rounded-2xl shadow-lg">
            <div className="flex items-center justify-between mb-6">
              <h3 className="text-lg font-semibold text-gray-100">Recent Transactions</h3>
              <Link
                to="/records"
                className="text-blue-400 text-sm hover:text-blue-300"
              >
                See All
              </Link>
            </div>

            <div className="space-y-4">
              {recordsData
                .slice(1)
                .slice(-4)
                .map((record) => (
                  <Link
                    to="/records"
                    key={record.id}
                    className="block bg-gray-700 p-4 rounded-xl flex items-center justify-between hover:bg-gray-600 transition-colors"
                  >
                    <div>
                      <p className="text-gray-100 font-medium">{record.categoryName}</p>
                      <p className="text-gray-400 text-sm">
                        {DateTime.fromISO(record.recordDate).toLocaleString(DateTime.DATE_MED)}
                      </p>
                    </div>
                    <span className={`text-sm font-semibold ${record.amount > 0 ? "text-green-400" : "text-red-400"}`}>
                      {getCurrencyLabel("BGN", record.amount)}
                    </span>
                  </Link>
                ))}
            </div>
          </div>
        </div>

        {/* Bottom Row */}
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 mt-8">
          {/* Expense Categories */}
          <div className="bg-gray-800 p-6 rounded-2xl shadow-lg">
            <h3 className="text-lg font-semibold text-gray-100 mb-6">Expense Categories</h3>
            <div className="space-y-6">
              {getTopExpenseCategories().map((category, idx, arr) => (
                <div key={category.name} className="space-y-2">
                  <div className="flex items-center justify-between">
                    <span className="text-gray-300 text-sm">{category.name}</span>
                    <span className="text-gray-400 text-sm">{getCurrencyLabel("BGN", category.amount)}</span>
                  </div>
                  <div className="w-full bg-gray-700 rounded-full h-2">
                    <div
                      className={`${category.color} h-2 rounded-full`}
                      style={{
                        width: arr[0].amount > 0 ? `${Math.round((category.amount / arr[0].amount) * 100)}%` : "0%",
                      }}
                    />
                  </div>
                </div>
              ))}
            </div>
          </div>

          {/* Quick Add Expense */}
          <div className="bg-gray-800 p-6 rounded-2xl shadow-lg">
            <h3 className="text-lg font-semibold text-gray-100 mb-6">Quick Add Expense</h3>
            <div className="space-y-4">
              <input
                type="text" // Changed input type to text
                placeholder="Enter a description or amount (e.g., 'Lunch $15')"
                value={quickAddText} // Updated to use quickAddText state
                onChange={(e) => setQuickAddText(e.target.value)} // Updated handler
                className="w-full bg-gray-700 border border-gray-600 rounded-xl px-4 py-3
                  text-gray-100 placeholder-gray-500 focus:outline-none focus:border-blue-400
                  focus:ring-2 focus:ring-blue-400/30 transition-all"
              />
              <button
                onClick={handleQuickAdd}
                className="w-full bg-blue-500 hover:bg-blue-400 text-white py-3 rounded-xl
                font-medium flex items-center justify-center gap-2 transition-colors"
              >
                <PlusIcon className="h-5 w-5" />
                Generate Record
              </button>
            </div>
          </div>
        </div>

        {/* AI-Generated Record Modal */}
        {isModalOpen && (
          <Modal onClose={() => setIsModalOpen(false)}>
            {aiGeneratedRecord ? (
              <>
                <h3 className="text-lg font-semibold text-gray-100 mb-4 flex items-center gap-2">
                  <SparklesIcon className="h-5 w-5 text-yellow-400" />
                  AI-Generated Record
                </h3>
                <table className="min-w-full table-auto text-sm text-left text-gray-400">
                  <thead className="bg-gray-700 text-gray-300 uppercase text-xs font-medium">
                    <tr>
                      <th className="px-4 py-2">Category</th>
                      <th className="px-4 py-2">Account</th>
                      <th className="px-4 py-2">From Account</th>
                      <th className="px-4 py-2">Amount</th>
                      <th className="px-4 py-2">Date</th>
                      <th className="px-4 py-2">Notes</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr className="border-b border-gray-700">
                      <td className="px-4 py-2">{aiGeneratedRecord.categoryName}</td>
                      <td className="px-4 py-2">{aiGeneratedRecord.accountName}</td>
                      <td className="px-4 py-2">{aiGeneratedRecord.fromAccountName || "-"}</td>
                      <td className="px-4 py-2">{aiGeneratedRecord.amount.toFixed(2)}</td>
                      <td className="px-4 py-2">{new Date(aiGeneratedRecord.recordDate).toLocaleDateString()}</td>
                      <td className="px-4 py-2">{aiGeneratedRecord.note}</td>
                    </tr>
                  </tbody>
                </table>
                <div className="mt-4 flex justify-end">
                  <button
                    onClick={handleConfirmRecord}
                    className="bg-green-500 hover:bg-green-400 text-white px-4 py-2 rounded-lg text-sm"
                  >
                    Confirm
                  </button>
                </div>
              </>
            ) : (
              <div className="text-center">
                <h3 className="text-lg font-semibold text-gray-100 mb-4">AI Failed to Generate a Record</h3>
                <p className="text-gray-400 mb-6">
                  The AI could not generate a record based on your input. Please try again with a different description.
                </p>
                <button
                  onClick={() => setIsModalOpen(false)}
                  className="bg-blue-500 hover:bg-blue-400 text-white px-4 py-2 rounded-lg text-sm"
                >
                  Close
                </button>
              </div>
            )}
          </Modal>
        )}
      </main>
    </div>
  );
};

export default DashboardPage;
