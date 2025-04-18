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
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchCategories = async () => {
      const categoriesResponse = await getCategories(axiosAuth);
      const typesResponse = await getCategoryTypes(axiosAuth);
      setCategories(categoriesResponse);
      setCategoryTypes(typesResponse);
      const category = categoriesResponse.find((cat) => cat.id === id);
      if (category) {
        setInitialData(category);
      }

      setIsLoading(false); // Set loading to false after data is fetched
    };

    fetchCategories();
  }, [id]);

  const handleEditCategory = async (categoryData) => {
    await updateCategory(categoryData, axiosAuth);
    navigate("/categories");
  };

  return isLoading ? ( // Conditionally render loading or form
    <p className="text-gray-400">Loading...</p>
  ) : (
    <CategoryForm
      category={initialData}
      onSubmit={handleEditCategory}
      categories={categories}
      categoryTypes={categoryTypes}
    />
  );
};

export default EditCategoryPage;
