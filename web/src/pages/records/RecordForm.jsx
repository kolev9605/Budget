import { useState } from "react";
import {
  CurrencyDollarIcon,
  DocumentTextIcon,
  ArrowsRightLeftIcon,
  ChevronLeftIcon,
} from "@heroicons/react/24/outline";
import { useNavigate } from "react-router-dom";

const RecordForm = ({ accounts, categories, recordTypes, onSubmit, record }) => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState(record);

  const handleSubmit = async (e) => {
    e.preventDefault();
    const recordData = {
      ...formData,
      amount: parseFloat(formData.amount) ?? 0,
      recordDate: new Date(formData.recordDate).toISOString(),
    };

    onSubmit(recordData);
  };

  return (
    <div className="min-h-screen bg-gray-900 p-6 sm:p-8 lg:p-10">
      <div className="max-w-2xl mx-auto">
        <div className="bg-gray-800 p-6 rounded-2xl shadow-xl">
          <div className="flex items-center mb-6">
            <button
              onClick={() => navigate(-1)}
              className="flex items-center text-gray-400 hover:text-blue-400 px-3 py-2 rounded-lg transition-colors bg-gray-800 hover:bg-gray-700 shadow-md"
            >
              <ChevronLeftIcon className="h-5 w-5" />
            </button>
            <h1 className="text-2xl font-bold text-gray-100 ml-2">
              {record?.id ? "Edit Record" : "Create New Record"}
            </h1>
          </div>

          <form onSubmit={handleSubmit} className="space-y-6">
            {/* Transaction Type Selector */}
            <div className="grid grid-cols-3 gap-4">
              {recordTypes.map((recordType) => (
                <button
                  key={recordType}
                  type="button"
                  onClick={() => setFormData({ ...formData, recordType: recordType })}
                  className={`p-3 rounded-xl flex items-center justify-center gap-2 transition-colors
                    ${
                      formData.recordType === recordType
                        ? "bg-blue-500 text-white"
                        : "bg-gray-700 hover:bg-gray-600 text-gray-300"
                    }`}
                >
                  {recordType === "Transfer" ? (
                    <ArrowsRightLeftIcon className="h-5 w-5" />
                  ) : (
                    <CurrencyDollarIcon className="h-5 w-5" />
                  )}
                  {recordType.charAt(0).toUpperCase() + recordType.slice(1)}
                </button>
              ))}
            </div>

            {/* Amount and Date */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label className="block text-sm font-medium text-gray-300 mb-3">Amount</label>
                <div className="relative">
                  <input
                    type="number"
                    step="0.01"
                    value={formData.amount}
                    onChange={(e) => setFormData({ ...formData, amount: e.target.value })}
                    className="w-full bg-gray-700 border border-gray-600 rounded-xl pl-12 pr-4 py-3.5
                      text-gray-100 placeholder-gray-500 focus:outline-none focus:border-blue-400
                      focus:ring-2 focus:ring-blue-400/30 transition-all"
                    placeholder="0.00"
                  />
                  <span className="absolute left-4 top-1/2 -translate-y-1/2 text-gray-400">$</span>
                </div>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-300 mb-3">Date</label>
                <input
                  type="date"
                  value={formData.recordDate}
                  onChange={(e) => setFormData({ ...formData, recordDate: e.target.value })}
                  className="w-full bg-gray-700 border border-gray-600 rounded-xl px-4 py-3.5
                    text-gray-100 focus:outline-none focus:border-blue-400 focus:ring-2 
                    focus:ring-blue-400/30"
                />
              </div>
            </div>

            {/* Accounts Section */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label className="block text-sm font-medium text-gray-300 mb-3">
                  {formData.recordType === "transfer" ? "From Account" : "Account"}
                </label>
                <select
                  value={formData.accountId}
                  onChange={(e) => setFormData({ ...formData, accountId: e.target.value })}
                  className="w-full bg-gray-700 border border-gray-600 rounded-xl px-4 py-3.5
                    text-gray-100 focus:outline-none focus:border-blue-400 focus:ring-2 
                    focus:ring-blue-400/30 appearance-none"
                >
                  <option value="">Select Account</option>
                  {accounts.map((account) => (
                    <option key={account.id} value={account.id}>
                      {account.name}
                    </option>
                  ))}
                </select>
              </div>

              {formData.recordType === "Transfer" && (
                <div>
                  <label className="block text-sm font-medium text-gray-300 mb-3">From Account</label>
                  <select
                    value={formData.fromAccountId}
                    onChange={(e) => setFormData({ ...formData, fromAccountId: e.target.value })}
                    className="w-full bg-gray-700 border border-gray-600 rounded-xl px-4 py-3.5
                      text-gray-100 focus:outline-none focus:border-blue-400 focus:ring-2 
                      focus:ring-blue-400/30 appearance-none"
                  >
                    <option value="">Select Source Account</option>
                    {accounts
                      .filter((acc) => acc.id !== formData.account)
                      .map((account) => (
                        <option key={account.id} value={account.id}>
                          {account.name}
                        </option>
                      ))}
                  </select>
                </div>
              )}
            </div>

            {/* Category Selection */}
            {formData.recordType !== "transfer" && (
              <div>
                <label className="block text-sm font-medium text-gray-300 mb-3">Category</label>
                <select
                  value={formData.categoryId}
                  onChange={(e) => setFormData({ ...formData, categoryId: e.target.value })}
                  className="w-full bg-gray-700 border border-gray-600 rounded-xl px-4 py-3.5
                    text-gray-100 focus:outline-none focus:border-blue-400 focus:ring-2 
                    focus:ring-blue-400/30 appearance-none"
                >
                  <option value="">Select Category</option>
                  {categories.map((category) => (
                    <option key={category.id} value={category.id}>
                      {category.name}
                    </option>
                  ))}
                </select>
              </div>
            )}

            {/* Note Field */}
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-3">Note</label>
              <div className="relative">
                <textarea
                  value={formData.note}
                  onChange={(e) => setFormData({ ...formData, note: e.target.value })}
                  className="w-full bg-gray-700 border border-gray-600 rounded-xl px-4 py-3.5
                    text-gray-100 placeholder-gray-500 focus:outline-none focus:border-blue-400
                    focus:ring-2 focus:ring-blue-400/30 transition-all resize-none"
                  placeholder="Add a note..."
                  rows="3"
                />
                <DocumentTextIcon className="h-5 w-5 text-gray-500 absolute top-4 right-4" />
              </div>
            </div>

            {/* Form Actions */}
            <div className="flex flex-col-reverse sm:flex-row gap-4 mt-8">
              <button
                type="button"
                onClick={() => navigate(-1)}
                className="w-full sm:w-auto px-6 py-3 text-gray-300 hover:text-white 
                  bg-gray-700 hover:bg-gray-600 rounded-xl transition-colors"
              >
                Cancel
              </button>
              <button
                type="submit"
                className="w-full sm:w-auto px-6 py-3 bg-blue-500 hover:bg-blue-400 
                  text-white rounded-xl flex items-center justify-center gap-2 transition-colors"
              >
                {record.id ? "Save Changes" : "Create Transaction"}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default RecordForm;
