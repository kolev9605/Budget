import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { InformationCircleIcon, XMarkIcon } from '@heroicons/react/24/outline';
import Layout from '../components/Layout';

const CategoryFormPage = ({ existingCategories = [] }) => {
  const { id } = useParams();
  const navigate = useNavigate();
  const isEditing = !!id;

  const [formData, setFormData] = useState({
    name: '',
    parentId: null, // Added for parent category selection
    description: '',
  });

  const [errors, setErrors] = useState({});

  useEffect(() => {
    if (isEditing) {
      // Load existing category data
      const sampleCategory = {
        id: 1,
        name: 'Food & Dining',
        color: '#3B82F6',
        description: 'Groceries, restaurants, etc.'
      };
      setFormData(sampleCategory);
    }
  }, [isEditing]);

  const validateForm = () => {
    const newErrors = {};
    
    if (!formData.name.trim()) newErrors.name = 'Category name is required';
    if (formData.name.length > 30) newErrors.name = 'Name must be less than 30 characters';
    
    // Check for duplicate names
    const duplicate = existingCategories.some(cat => 
      cat.name.toLowerCase() === formData.name.toLowerCase() && 
      (!isEditing || cat.id !== id)
    );
    if (duplicate) newErrors.name = 'Category name already exists';

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!validateForm()) return;

    const categoryData = {
      ...formData,
      id: isEditing ? id : crypto.randomUUID(),
    };

    console.log('Saving category:', categoryData);
    navigate('/categories');
  };

  return (
    <Layout>
    <div className="min-h-screen bg-gray-900 p-6 sm:p-8 lg:p-10">
      <div className="max-w-2xl mx-auto">
        <div className="bg-gray-800 p-6 rounded-2xl shadow-xl">
          <div className="flex items-center justify-between mb-6">
            <h1 className="text-2xl font-bold text-gray-100">
              {isEditing ? 'Edit Category' : 'Create New Category'}
            </h1>
            <button 
              onClick={() => navigate(-1)}
              className="text-gray-400 hover:text-blue-400 p-2 rounded-lg transition-colors"
            >
              <XMarkIcon className="h-6 w-6" />
            </button>
          </div>

          {Object.keys(errors).length > 0 && (
            <div className="bg-red-500/20 p-4 rounded-xl mb-6">
              <p className="text-red-400 text-sm flex items-center gap-2">
                <InformationCircleIcon className="h-5 w-5" />
                Please fix the following issues:
              </p>
              <ul className="list-disc list-inside mt-2 ml-5">
                {Object.values(errors).map((error, index) => (
                  <li key={index} className="text-red-300 text-sm">{error}</li>
                ))}
              </ul>
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-6">
            {/* Category Name */}
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-3">
                Category Name
              </label>
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

            {/* Parent Category Selector */}
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-3">
                Parent Category (Optional)
              </label>
              <select
                value={formData.parentId || ''}
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

            {/* Description */}
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-3">
                Description (Optional)
              </label>
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
                {isEditing ? 'Save Changes' : 'Create Category'}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
    </Layout>
  );
};

export default CategoryFormPage;