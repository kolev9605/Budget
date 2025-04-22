import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import CategoryForm from "./CategoryForm.jsx";
import { getCategories, getCategoryTypes, updateCategory } from "../../api/categories.service.js";
import LoadingOverlay from "../../components/LoadingOverlay.jsx";

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
        const categoriesResponse = await getCategories();
        const typesResponse = await getCategoryTypes();
        setCategories(categoriesResponse);
        setCategoryTypes(typesResponse);
        const category = categoriesResponse.find((cat) => cat.id === id);
        if (category) {
          setInitialData(category);
        }
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
