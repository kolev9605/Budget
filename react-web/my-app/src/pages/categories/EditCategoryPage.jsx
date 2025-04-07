import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import CategoryFormFields from "./CategoryFormFields";

const EditCategoryPage = ({ existingCategories }) => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [initialData, setInitialData] = useState(null);
  const sampleCategories = [
    { id: 1, name: "Shopping", parentId: null, description: "General shopping expenses" },
    { id: 2, name: "Clothes", parentId: 1, description: "Clothing and accessories" },
    { id: 7, name: "Home", parentId: 1, description: "Home stuff" },
    { id: 3, name: "Food & Dining", parentId: null, description: "Groceries, restaurants, etc." },
    { id: 4, name: "Transportation", parentId: null, description: "Fuel, public transport, etc." },
  ];

  useEffect(() => {
    // Find the category by ID
    const category = sampleCategories.find((cat) => cat.id === id);
    if (category) {
      setInitialData(category);
    } else {
      // navigate("/categories"); // Redirect if category not found
    }
  }, [id, sampleCategories, navigate]);

  const handleEditCategory = (categoryData) => {
    console.log("Editing category:", categoryData);
    // navigate("/categories");
  };

  if (!initialData) return null; // Wait for data to load

  return (
    <div className="min-h-screen bg-gray-900 p-6 sm:p-8 lg:p-10">
      <div className="max-w-2xl mx-auto">
        <div className="bg-gray-800 p-6 rounded-2xl shadow-xl">
          <h1 className="text-2xl font-bold text-gray-100 mb-6">Edit Category</h1>
          <CategoryFormFields
            initialData={initialData}
            onSubmit={handleEditCategory}
            existingCategories={sampleCategories}
          />
        </div>
      </div>
    </div>
  );
};

export default EditCategoryPage;
