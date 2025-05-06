import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import CategoryForm from "./CategoryForm.jsx";
import { getCategories, getCategoryTypes, updateCategory, getCategoryById } from "../../api/categories.service.js";
import LoadingOverlay from "../../components/LoadingOverlay.jsx";
import { toast } from "react-toastify";

const EditCategoryPage = () => {
  const { id } = useParams();
  const [categories, setCategories] = useState([]);
  const [categoryTypes, setCategoryTypes] = useState([]);
  const [initialData, setInitialData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const categoriesResponse = await getCategories(true); // Explicitly pass "true" as a string
        const typesResponse = await getCategoryTypes();
        const categoryResponse = await getCategoryById(id);
        setCategories(categoriesResponse);
        setCategoryTypes(typesResponse);
        setInitialData(categoryResponse);
      } finally {
        setIsLoading(false);
      }
    };

    fetchCategories();
  }, [id]);

  const handleEditCategory = async (categoryData) => {
    try {
      setIsLoading(true);
      await updateCategory(categoryData);
    } finally {
      setIsLoading(false);
    }

    toast.success("Category updated successfully!");
    navigate("/categories");
  };

  return isLoading ? (
    <LoadingOverlay />
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
