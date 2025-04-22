import { useNavigate } from "react-router-dom";
import CategoryForm from "./CategoryForm.jsx";
import { createCategory, getCategories, getCategoryTypes } from "../../api/categories.service.js";
import { useEffect, useState } from "react";
import LoadingOverlay from "../../components/LoadingOverlay.jsx";

const AddCategoryPage = () => {
  const [categories, setCategories] = useState([]);
  const [categoryTypes, setCategoryTypes] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const categoriesResponse = await getCategories();
        const typesResponse = await getCategoryTypes();
        setCategories(categoriesResponse);
        setCategoryTypes(typesResponse);
      } finally {
        setIsLoading(false);
      }
    };

    fetchCategories();
  }, []);

  const handleAddCategory = async (categoryData) => {
    try {
      setIsLoading(true);
      const newCategory = { ...categoryData };
      await createCategory(newCategory);
      console.log("Adding category:", newCategory);
    } finally {
      setIsLoading(false);
    }
    navigate("/categories");
  };

  return isLoading ? (
    <LoadingOverlay />
  ) : (
    <CategoryForm
      category={{ name: "", categoryType: "Need" }}
      onSubmit={handleAddCategory}
      categories={categories}
      categoryTypes={categoryTypes}
    />
  );
};

export default AddCategoryPage;
