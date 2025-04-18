import { useNavigate } from "react-router-dom";
import CategoryForm from "./CategoryForm.jsx";
import { createCategory, getCategories, getCategoryTypes } from "../../api/categories.service.js";
import { useEffect, useState } from "react";

const AddCategoryPage = () => {
  const [categories, setCategories] = useState([]);
  const [categoryTypes, setCategoryTypes] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchCategories = async () => {
      const categoriesResponse = await getCategories();
      const typesResponse = await getCategoryTypes();
      setCategories(categoriesResponse);
      setCategoryTypes(typesResponse);
    };

    fetchCategories();
  }, []);

  const handleAddCategory = async (categoryData) => {
    const newCategory = {
      ...categoryData,
    };

    await createCategory(newCategory);

    console.log("Adding category:", newCategory);
    navigate("/categories");
  };

  return (
    <CategoryForm
      category={{ name: "", categoryType: "Need" }}
      onSubmit={handleAddCategory}
      categories={categories}
      categoryTypes={categoryTypes}
    />
  );
};

export default AddCategoryPage;
