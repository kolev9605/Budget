import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import CategoryForm from "./CategoryForm.jsx";
import { getCategories, getCategoryTypes, updateCategory } from "../../api/categories.service.js";
import { useAuthContext } from "../../hooks/useAuthContext.js";
import { createAxiosAuth } from "../../api/createAxiosAuth.js";

const EditCategoryPage = () => {
  const { id } = useParams();
  const [categories, setCategories] = useState([]);
  const [categoryTypes, setCategoryTypes] = useState([]);
  const navigate = useNavigate();
  const [initialData, setInitialData] = useState(null);
  const { user } = useAuthContext();
  const axiosAuth = createAxiosAuth(user?.token);

  useEffect(() => {
    const fetchCategories = async () => {
      const categoriesResponse = await getCategories(axiosAuth);
      const typesResponse = await getCategoryTypes(axiosAuth);
      setCategories(categoriesResponse);
      setCategoryTypes(typesResponse);
      const category = categoriesResponse.find((cat) => cat.id === id);
      if (category) {
        setInitialData(category);
      } else {
        // navigate("/categories"); // Redirect if category not found
      }
    };

    fetchCategories();
  }, [id]);

  const handleEditCategory = async (categoryData) => {
    await updateCategory(categoryData, axiosAuth);
    navigate("/categories");
  };

  if (!initialData) return null; // Wait for data to load

  return (
    <div className="min-h-screen bg-gray-900 p-6 sm:p-8 lg:p-10">
      <div className="max-w-2xl mx-auto">
        <div className="bg-gray-800 p-6 rounded-2xl shadow-xl">
          <h1 className="text-2xl font-bold text-gray-100 mb-6">Edit Category</h1>
          <CategoryForm
            initialData={initialData}
            onSubmit={handleEditCategory}
            categories={categories}
            categoryTypes={categoryTypes}
          />
        </div>
      </div>
    </div>
  );
};

export default EditCategoryPage;
