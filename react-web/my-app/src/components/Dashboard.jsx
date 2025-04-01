import { useState } from "react";
import { ArrowUpIcon, ArrowDownIcon, CurrencyDollarIcon, ChartBarIcon, PlusIcon } from "@heroicons/react/24/outline";
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer } from "recharts";
import Layout from "./Layout";

const Dashboard = () => {
  const [quickAddAmount, setQuickAddAmount] = useState("");

  // Example data - replace with real data
  const dashboardData = {
    balance: 45250.75,
    income: 75000.0,
    expenses: 29749.25,
    categories: [
      { name: "Food & Drinks", amount: 8450, color: "bg-blue-400" },
      { name: "Shopping", amount: 6200, color: "bg-purple-400" },
      { name: "Bills", amount: 4500, color: "bg-green-400" },
      { name: "Transport", amount: 3200, color: "bg-yellow-400" },
    ],
    recentTransactions: [
      { id: 1, name: "Grocery Store", date: "2024-03-15", amount: -245.75, category: "Food" },
      { id: 2, name: "Salary Deposit", date: "2024-03-14", amount: 5000.0, category: "Income" },
      { id: 3, name: "Internet Bill", date: "2024-03-13", amount: -89.99, category: "Bills" },
      { id: 4, name: "Coffee Shop", date: "2024-03-13", amount: -12.5, category: "Food" },
    ],
    monthlyData: [
      { month: "Jan", income: 6500, expenses: 4200 },
      { month: "Feb", income: 7200, expenses: 4800 },
      { month: "Mar", income: 8000, expenses: 5200 },
      { month: "Apr", income: 7500, expenses: 4900 },
    ],
  };

  return (
    <Layout>
      <div className="min-h-screen bg-gray-900">
        {/* Navbar - Use the previous navbar component */}

        <main className="p-6 sm:p-8 lg:p-10 max-w-7xl mx-auto">
          {/* Quick Stats Row */}
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
            <div className="bg-gray-800 p-6 rounded-2xl shadow-lg">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-gray-400 text-sm mb-1">Total Balance</p>
                  <p className="text-2xl font-bold text-gray-100">${dashboardData.balance.toLocaleString()}</p>
                </div>
                <CurrencyDollarIcon className="h-8 w-8 text-blue-400" />
              </div>
            </div>

            <div className="bg-gray-800 p-6 rounded-2xl shadow-lg">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-gray-400 text-sm mb-1">Total Income</p>
                  <p className="text-2xl font-bold text-green-400">+${dashboardData.income.toLocaleString()}</p>
                </div>
                <ArrowUpIcon className="h-8 w-8 text-green-400" />
              </div>
            </div>

            <div className="bg-gray-800 p-6 rounded-2xl shadow-lg">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-gray-400 text-sm mb-1">Total Expenses</p>
                  <p className="text-2xl font-bold text-red-400">-${dashboardData.expenses.toLocaleString()}</p>
                </div>
                <ArrowDownIcon className="h-8 w-8 text-red-400" />
              </div>
            </div>

            <div className="bg-gray-800 p-6 rounded-2xl shadow-lg">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-gray-400 text-sm mb-1">Savings Rate</p>
                  <p className="text-2xl font-bold text-blue-400">
                    {((1 - dashboardData.expenses / dashboardData.income) * 100).toFixed(1)}%
                  </p>
                </div>
                <ChartBarIcon className="h-8 w-8 text-blue-400" />
              </div>
            </div>
          </div>

          {/* Main Content */}
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            {/* Chart Section */}
            <div className="lg:col-span-2 bg-gray-800 p-6 rounded-2xl shadow-lg">
              <h3 className="text-lg font-semibold text-gray-100 mb-6">Monthly Overview</h3>
              <div className="h-80">
                <ResponsiveContainer width="100%" height="100%">
                  <BarChart data={dashboardData.monthlyData}>
                    <XAxis dataKey="month" stroke="#6B7280" />
                    <YAxis stroke="#6B7280" />
                    <Tooltip
                      contentStyle={{ backgroundColor: "#1F2937", border: "none" }}
                      itemStyle={{ color: "#F9FAFB" }}
                    />
                    <Bar dataKey="income" fill="#60A5FA" radius={[4, 4, 0, 0]} name="Income" />
                    <Bar dataKey="expenses" fill="#F87171" radius={[4, 4, 0, 0]} name="Expenses" />
                  </BarChart>
                </ResponsiveContainer>
              </div>
            </div>

            {/* Recent Transactions */}
            <div className="bg-gray-800 p-6 rounded-2xl shadow-lg">
              <div className="flex items-center justify-between mb-6">
                <h3 className="text-lg font-semibold text-gray-100">Recent Transactions</h3>
                <button className="text-blue-400 text-sm hover:text-blue-300">See All</button>
              </div>

              <div className="space-y-4">
                {dashboardData.recentTransactions.map((transaction) => (
                  <div
                    key={transaction.id}
                    className="bg-gray-700 p-4 rounded-xl flex items-center justify-between hover:bg-gray-600 transition-colors"
                  >
                    <div>
                      <p className="text-gray-100 font-medium">{transaction.name}</p>
                      <p className="text-gray-400 text-sm">{transaction.date}</p>
                    </div>
                    <span
                      className={`text-sm font-semibold ${transaction.amount > 0 ? "text-green-400" : "text-red-400"}`}
                    >
                      ${Math.abs(transaction.amount).toFixed(2)}
                    </span>
                  </div>
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
                {dashboardData.categories.map((category) => (
                  <div key={category.name} className="space-y-2">
                    <div className="flex items-center justify-between">
                      <span className="text-gray-300 text-sm">{category.name}</span>
                      <span className="text-gray-400 text-sm">${category.amount.toLocaleString()}</span>
                    </div>
                    <div className="w-full bg-gray-700 rounded-full h-2">
                      <div
                        className={`${category.color} h-2 rounded-full`}
                        style={{ width: `${(category.amount / dashboardData.expenses) * 100}%` }}
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
                  type="number"
                  placeholder="Amount"
                  value={quickAddAmount}
                  onChange={(e) => setQuickAddAmount(e.target.value)}
                  className="w-full bg-gray-700 border border-gray-600 rounded-xl px-4 py-3
                  text-gray-100 placeholder-gray-500 focus:outline-none focus:border-blue-400
                  focus:ring-2 focus:ring-blue-400/30 transition-all"
                />
                <button
                  className="w-full bg-blue-500 hover:bg-blue-400 text-white py-3 rounded-xl
                font-medium flex items-center justify-center gap-2 transition-colors"
                >
                  <PlusIcon className="h-5 w-5" />
                  Add Expense
                </button>
              </div>
            </div>
          </div>
        </main>
      </div>
    </Layout>
  );
};

export default Dashboard;
