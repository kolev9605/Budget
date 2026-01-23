import { useState, useEffect } from "react";
import {
  CurrencyDollarIcon,
  DocumentTextIcon,
  ArrowsRightLeftIcon,
  ChevronLeftIcon,
} from "@heroicons/react/24/outline";
import { useNavigate } from "react-router-dom";
import { getMostUsedCategories } from "../../api/categories.service";

const RecordForm = ({ accounts, categories, recordTypes, onSubmit, record }) => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState(record);

  // Add state for expanded parent category, collapsed by default
  const [expandedParentCategory, setExpandedParentCategory] = useState(null);

  // State for most used categories
  const [mostUsedCategories, setMostUsedCategories] = useState([]);

  // Fetch most used categories on mount
  useEffect(() => {
    getMostUsedCategories().then((data) => {
      // Map API result (with id) to full category objects from categories prop
      const mapped = data.map((item) => categories.find((cat) => cat.id === item.id)).filter(Boolean);
      setMostUsedCategories(mapped);

      // Preselect the most used category if not already selected
      if (!formData.categoryId && mapped.length > 0) {
        setFormData((prev) => ({
          ...prev,
          categoryId: mapped[0].id,
        }));
      }
    });
  }, [categories]);

  // Find the transfer category if present
  const transferCategory = categories.find(
    (cat) => cat.name?.toLowerCase?.() === "transfer" && cat.categoryType?.toLowerCase?.() === "transfer"
  );

  // Store the most used category id for reuse
  const mostUsedCategoryId = mostUsedCategories.length > 0 ? mostUsedCategories[0].id : null;

  // Handle record type change and set category accordingly
  const handleRecordTypeChange = (recordType) => {
    if (recordType === "Transfer" && transferCategory) {
      setFormData((prev) => ({
        ...prev,
        recordType,
        categoryId: transferCategory.id,
      }));
    } else {
      setFormData((prev) => ({
        ...prev,
        recordType,
        categoryId: mostUsedCategoryId ?? null,
      }));
    }
  };

  // Helper to group categories by parent (assuming category.parentCategoryId)
  const parentCategories = categories.filter((cat) => !cat.parentCategoryId);
  const getSubCategories = (parentId) => categories.filter((cat) => cat.parentCategoryId === parentId);

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
                  onClick={() => handleRecordTypeChange(recordType)}
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

            {/* Most Used Categories */}
            {formData.recordType !== "Transfer" && (
              <div>
                <div className="mb-2 mt-6 text-gray-300 font-semibold text-sm">Most Used Categories</div>
                <div className="grid grid-cols-2 sm:grid-cols-4 gap-4 mb-4">
                  {mostUsedCategories.length > 0 ? (
                    mostUsedCategories.slice(0, 4).map((category) => (
                      <button
                        key={category.id}
                        type="button"
                        onClick={() => setFormData({ ...formData, categoryId: category.id })}
                        className={`flex flex-col items-center justify-center p-4 rounded-xl shadow-md transition-colors
                            ${
                              formData.categoryId === category.id
                                ? "bg-blue-500 text-white"
                                : "bg-gray-700 hover:bg-gray-600 text-gray-300"
                            }`}
                      >
                        <span className="font-medium text-sm">{category.name}</span>
                      </button>
                    ))
                  ) : (
                    <span className="text-gray-400 col-span-4">No usage data</span>
                  )}
                </div>
              </div>
            )}

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
                  type="datetime-local"
                  value={formData.recordDate ? new Date(formData.recordDate).toISOString().slice(0, 16) : ""}
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
            {formData.recordType !== "Transfer" && (
              <div>
                <label className="block text-sm font-medium text-gray-300 mb-3">Category</label>
                {/* Collapsible Category Selector */}
                <div className="bg-gray-700 rounded-xl p-2">
                  {/* Collapsed view: show selected category and expand button */}
                  {expandedParentCategory === null && (
                    <div className="flex items-center">
                      <span className="flex-1 px-4 py-2 rounded-lg bg-gray-800 text-gray-100">
                        {categories.find((cat) => cat.id === formData.categoryId)?.name || (
                          <span className="text-gray-400">No category selected</span>
                        )}
                      </span>
                      <button
                        type="button"
                        className="ml-2 px-2 py-2 rounded transition-colors text-xs text-gray-400 hover:text-blue-400"
                        onClick={() => setExpandedParentCategory("__expand__")}
                        aria-label="Expand categories"
                      >
                        ▼
                      </button>
                    </div>
                  )}
                  {/* Expanded view: show all categories and allow selection */}
                  {expandedParentCategory !== null && (
                    <div>
                      {parentCategories.map((parent) => (
                        <div key={parent.id} className="mb-2">
                          {/* Parent category is always selectable */}
                          <div className="flex items-center">
                            <button
                              type="button"
                              className={`flex-1 text-left px-4 py-2 rounded-lg transition-colors
                                ${
                                  formData.categoryId === parent.id
                                    ? "bg-blue-500 text-white"
                                    : "bg-gray-800 hover:bg-gray-600 text-gray-200"
                                }`}
                              onClick={() => {
                                setFormData({ ...formData, categoryId: parent.id });
                                setExpandedParentCategory(null);
                              }}
                            >
                              {parent.name}
                            </button>
                            {getSubCategories(parent.id).length > 0 && (
                              <button
                                type="button"
                                className="ml-2 px-2 py-2 rounded transition-colors text-xs text-gray-400 hover:text-blue-400"
                                onClick={() =>
                                  setExpandedParentCategory(
                                    expandedParentCategory === parent.id ? "__expand__" : parent.id
                                  )
                                }
                                aria-label={
                                  expandedParentCategory === parent.id
                                    ? "Collapse subcategories"
                                    : "Expand subcategories"
                                }
                              >
                                {expandedParentCategory === parent.id ? "▲" : "▼"}
                              </button>
                            )}
                          </div>
                          {/* Sub-categories */}
                          {expandedParentCategory === parent.id && getSubCategories(parent.id).length > 0 && (
                            <div className="ml-4 mt-1">
                              {getSubCategories(parent.id).map((sub) => (
                                <button
                                  key={sub.id}
                                  type="button"
                                  className={`w-full text-left px-4 py-2 rounded-lg mt-1 transition-colors
                                    ${
                                      formData.categoryId === sub.id
                                        ? "bg-blue-400 text-white"
                                        : "bg-gray-600 hover:bg-gray-500 text-gray-100"
                                    }`}
                                  onClick={() => {
                                    setFormData({ ...formData, categoryId: sub.id });
                                    setExpandedParentCategory(null);
                                  }}
                                >
                                  {sub.name}
                                </button>
                              ))}
                            </div>
                          )}
                        </div>
                      ))}
                      {/* Collapse button */}
                      <div className="flex justify-end">
                        <button
                          type="button"
                          className="mt-2 px-3 py-1 rounded text-xs text-gray-400 hover:text-blue-400"
                          onClick={() => setExpandedParentCategory(null)}
                        >
                          Collapse
                        </button>
                      </div>
                      {/* Fallback if no categories */}
                      {categories.length === 0 && (
                        <div className="text-gray-400 px-4 py-2">No categories available</div>
                      )}
                    </div>
                  )}
                </div>
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
