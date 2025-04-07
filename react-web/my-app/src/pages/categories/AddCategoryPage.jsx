import { useNavigate } from "react-router-dom";
import CategoryFormFields from "./CategoryFormFields.jsx";

const AddCategoryPage = () => {
  const navigate = useNavigate();
    const sampleCategories = [
    { id: 1, name: "Shopping", parentId: null, description: "General shopping expenses" },
    { id: 2, name: "Clothes", parentId: 1, description: "Clothing and accessories" },
    { id: 7, name: "Home", parentId: 1, description: "Home stuff" },
    { id: 3, name: "Food & Dining", parentId: null, description: "Groceries, restaurants, etc." },
    { id: 4, name: "Transportation", parentId: null, description: "Fuel, public transport, etc." },
  ];

  const handleAddCategory = (categoryData) => {
    const newCategory = {
      ...categoryData,
      id: crypto.randomUUID(),
    };
    console.log("Adding category:", newCategory);
    navigate("/categories");
  };

  return (
    <div className="min-h-screen bg-gray-900 p-6 sm:p-8 lg:p-10">
      <div className="max-w-2xl mx-auto">
        <div className="bg-gray-800 p-6 rounded-2xl shadow-xl">
          <h1 className="text-2xl font-bold text-gray-100 mb-6">Create New Category</h1>
          <CategoryFormFields
            initialData={{ name: "", parentId: null, description: "" }}
            onSubmit={handleAddCategory}
            existingCategories={sampleCategories}
          />
        </div>
      </div>
    </div>
  );
};

export default AddCategoryPage;
