import { useNavigate } from "react-router-dom";
import CategoryForm from "./CategoryForm.jsx";
import { createCategory, getCategories, getCategoryTypes } from "../../api/categories.service.js";
import { useEffect, useState } from "react";
import { useAuthContext } from "../../hooks/useAuthContext.js";
import { createAxiosAuth } from "../../api/createAxiosAuth.js";

const AddCategoryPage = () => {
  const [categories, setCategories] = useState([]);
  const [categoryTypes, setCategoryTypes] = useState([]);
  const { user } = useAuthContext();
  const axiosAuth = createAxiosAuth(user?.token);

  const navigate = useNavigate();

  useEffect(() => {
    const fetchCategories = async () => {
      const categoriesResponse = await getCategories(axiosAuth);
      const typesResponse = await getCategoryTypes(axiosAuth);
      setCategories(categoriesResponse);
      setCategoryTypes(typesResponse);
    };

    fetchCategories();
  }, []);

  const handleAddCategory = async (categoryData) => {
    const newCategory = {
      ...categoryData,
    };

    await createCategory(newCategory, axiosAuth);

    console.log("Adding category:", newCategory);
    navigate("/categories");
  };

  return (
    <div className="min-h-screen bg-gray-900 p-6 sm:p-8 lg:p-10">
      <div className="max-w-2xl mx-auto">
        <div className="bg-gray-800 p-6 rounded-2xl shadow-xl">
          <h1 className="text-2xl font-bold text-gray-100 mb-6">Create New Category</h1>
          <CategoryForm
            initialData={{ name: "", parentCategoryId: null, categoryType: "" }}
            onSubmit={handleAddCategory}
            categories={categories}
            categoryTypes={categoryTypes}
          />
        </div>
      </div>
    </div>
  );
};

export default AddCategoryPage;
