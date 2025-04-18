import { useState } from "react";
import { InformationCircleIcon } from "@heroicons/react/24/outline";

const CategoryForm = ({ category, onSubmit, categories, categoryTypes }) => {
  const [formData, setFormData] = useState(category);
  const [errors, setErrors] = useState({});

  const validateForm = () => {
    const newErrors = {};
    if (!formData.name.trim()) newErrors.name = "Category name is required";
    if (formData.name.length > 30) newErrors.name = "Name must be less than 30 characters";
    if (!formData.categoryType.trim()) newErrors.categoryType = "Category type is required";

    const duplicate = categories.some(
      (cat) => cat.name.toLowerCase() === formData.name.toLowerCase() && cat.id !== formData.id
    );

    if (duplicate) newErrors.name = "Category name already exists";

    setErrors(newErrors);

    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!validateForm()) return;
    onSubmit(formData);
  };

  return (
    <div className="min-h-screen bg-gray-900 p-6 sm:p-8 lg:p-10">
      <div className="max-w-2xl mx-auto">
        <div className="bg-gray-800 p-6 rounded-2xl shadow-xl">
          <h1 className="text-2xl font-bold text-gray-100 mb-6">{category?.id ? "Edit Category" : "Create New Category"}</h1>
          <form onSubmit={handleSubmit} className="space-y-6">
            {Object.keys(errors).length > 0 && (
              <div className="bg-red-500/20 p-4 rounded-xl mb-6">
                <p className="text-red-400 text-sm flex items-center gap-2">
                  <InformationCircleIcon className="h-5 w-5" />
                  Please fix the following issues:
                </p>
                <ul className="list-disc list-inside mt-2 ml-5">
                  {Object.values(errors).map((error, index) => (
                    <li key={index} className="text-red-300 text-sm">
                      {error}
                    </li>
                  ))}
                </ul>
              </div>
            )}

            <div>
              <label className="block text-sm font-medium text-gray-300 mb-3">Category Name</label>
              <input
                type="text"
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                className="w-full bg-gray-700 border border-gray-600 rounded-xl px-4 py-3.5
            text-gray-100 placeholder-gray-500 focus:outline-none focus:border-blue-400
            focus:ring-2 focus:ring-blue-400/30 transition-all"
                placeholder="Enter category name"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-300 mb-3">Category Type</label>
              <div className="grid grid-cols-2 sm:grid-cols-4 gap-4">
                {categoryTypes.map((categoryType) => (
                  <div
                    key={categoryType}
                    onClick={() => setFormData({ ...formData, categoryType: categoryType })}
                    className={`cursor-pointer p-4 rounded-lg shadow-md transition-transform transform flex flex-col items-center justify-center gap-2 ${
                      formData.categoryType === categoryType
                        ? "bg-blue-500 text-white scale-105"
                        : "bg-gray-800 text-gray-300 hover:bg-gray-700 hover:scale-105"
                    }`}
                  >
                    <span className="text-sm font-bold">{categoryType}</span>
                  </div>
                ))}
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-300 mb-3">Parent Category (Optional)</label>
              <select
                value={formData.parentCategoryId || ""}
                onChange={(e) => setFormData({ ...formData, parentCategoryId: e.target.value || null })}
                className="w-full bg-gray-700 border border-gray-600 rounded-xl px-4 py-3.5
            text-gray-100 placeholder-gray-500 focus:outline-none focus:border-blue-400
            focus:ring-2 focus:ring-blue-400/30 transition-all"
              >
                <option value="">None (Parent Category)</option>
                {categories.map((category) => (
                  <option key={category.id} value={category.id}>
                    {category.name}
                  </option>
                ))}
              </select>
            </div>

            <div className="flex flex-col-reverse sm:flex-row gap-4 mt-8">
              <button
                type="button"
                onClick={() => window.history.back()}
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
                Save
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default CategoryForm;
