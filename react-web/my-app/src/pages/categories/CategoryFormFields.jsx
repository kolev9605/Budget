import { useState } from "react";
import { InformationCircleIcon } from "@heroicons/react/24/outline";

const CategoryFormFields = ({ initialData, onSubmit, existingCategories }) => {
  const [formData, setFormData] = useState(initialData);
  const [errors, setErrors] = useState({});

  const validateForm = () => {
    const newErrors = {};
    if (!formData.name.trim()) newErrors.name = "Category name is required";
    if (formData.name.length > 30) newErrors.name = "Name must be less than 30 characters";

    const duplicate = existingCategories.some(
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
        <label className="block text-sm font-medium text-gray-300 mb-3">Parent Category (Optional)</label>
        <select
          value={formData.parentId || ""}
          onChange={(e) => setFormData({ ...formData, parentId: e.target.value || null })}
          className="w-full bg-gray-700 border border-gray-600 rounded-xl px-4 py-3.5
            text-gray-100 placeholder-gray-500 focus:outline-none focus:border-blue-400
            focus:ring-2 focus:ring-blue-400/30 transition-all"
        >
          <option value="">None (Parent Category)</option>
          {existingCategories.map((category) => (
            <option key={category.id} value={category.id}>
              {category.name}
            </option>
          ))}
        </select>
      </div>

      <div>
        <label className="block text-sm font-medium text-gray-300 mb-3">Description (Optional)</label>
        <textarea
          value={formData.description}
          onChange={(e) => setFormData({ ...formData, description: e.target.value })}
          className="w-full bg-gray-700 border border-gray-600 rounded-xl px-4 py-3.5
            text-gray-100 placeholder-gray-500 focus:outline-none focus:border-blue-400
            focus:ring-2 focus:ring-blue-400/30 transition-all resize-none"
          placeholder="Add a description..."
          rows="3"
        />
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
  );
};

export default CategoryFormFields;
