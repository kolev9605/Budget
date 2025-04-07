import { Link, NavLink } from "react-router-dom";
import {
  PlusIcon,
  PencilIcon,
  TrashIcon,
  ChevronDownIcon,
  ChevronRightIcon,
  FolderIcon,
} from "@heroicons/react/24/outline";
import { useEffect, useState } from "react";
import { getCategories } from "../../api/categories.service.js";
import { createAxiosAuth } from "../../api/createAxiosAuth.js";
import { useAuthContext } from "../../hooks/useAuthContext.js";

const CategoryPage = () => {
  // Sample data - replace with real data
  // const categories = [
  //   { id: 1, name: "Shopping", parentId: null, description: "General shopping expenses" },
  //   { id: 2, name: "Clothes", parentId: 1, description: "Clothing and accessories" },
  //   { id: 7, name: "Home", parentId: 1, description: "Home stuff" },
  //   { id: 3, name: "Food & Dining", parentId: null, description: "Groceries, restaurants, etc." },
  //   { id: 4, name: "Transportation", parentId: null, description: "Fuel, public transport, etc." },
  // ];

  const [categories, setCategories] = useState([]);
  const { user } = useAuthContext();
  const axiosAuth = createAxiosAuth(user?.token);

  const [collapsed, setCollapsed] = useState({});

  useEffect(() => {
    const fetchAccounts = async () => {
      const response = await getCategories(axiosAuth);
      setCategories(response);

      response.forEach((category) => {
        if (!category.parentCategoryId) {
          setCollapsed((prev) => ({ ...prev, [category.id]: true }));
        }
      });
    };

    fetchAccounts();
  }, []);

  const toggleCollapse = (categoryId) => {
    setCollapsed((prev) => ({ ...prev, [categoryId]: !prev[categoryId] }));
  };

  const renderCategory = (category, isParent = false) => {
    const hasSubCategories = categories.some((subCategory) => subCategory.parentCategoryId === category.id);

    return (
      <div
        key={category.id}
        className={`flex justify-between items-center bg-gray-800 p-4 shadow-md ${
          isParent
            ? collapsed[category.id]
              ? "rounded-lg" // Fully rounded when collapsed
              : "rounded-t-lg" // Remove bottom-right radius when expanded
            : "" /* No border radius for sub-categories */
        }`}
      >
        <div className="flex items-center gap-2">
          {isParent &&
            hasSubCategories && ( // Show icon only if there are sub-categories
              <button
                onClick={() => toggleCollapse(category.id)}
                className="text-gray-400 hover:text-gray-300 transition-colors"
              >
                {collapsed[category.id] ? (
                  <ChevronRightIcon className="h-4 w-4" /> // Points right when collapsed
                ) : (
                  <ChevronDownIcon className="h-4 w-4" /> // Points down when expanded
                )}
              </button>
            )}
          <div>
            <h3 className="text-sm font-semibold text-gray-100">{category.name}</h3>
            <p className="text-xs text-gray-400">{category.description}</p>
          </div>
        </div>
        <div className="flex items-center gap-2">
          <Link
            to={`/categories/edit/${category.id}`}
            className="text-gray-400 hover:text-blue-400 p-2 rounded-lg transition-colors"
          >
            <PencilIcon className="h-4 w-4" />
          </Link>
          <button className="text-gray-400 hover:text-red-400 p-2 rounded-lg transition-colors">
            <TrashIcon className="h-4 w-4" />
          </button>
        </div>
      </div>
    );
  };

  const renderCategories = (categories, parentCategoryId = null) => {
    return categories
      .filter((category) => category.parentCategoryId === parentCategoryId)
      .map((category) => (
        <div key={category.id}>
          {renderCategory(category, parentCategoryId === null)}
          {parentCategoryId === null && !collapsed[category.id] && (
            <div className="bg-gray-850 rounded-b-lg">{renderCategories(categories, category.id)}</div>
          )}
        </div>
      ));
  };

  return (
    <div className="min-h-screen bg-gray-900 p-6 sm:p-8 lg:p-10">
      {" "}
      {/* Ensures full page has a dark background */}
      <div className="max-w-7xl mx-auto">
        {" "}
        {/* Added max-w-4xl and mx-auto for consistent width */}
        <div className="flex items-center justify-between mb-6">
          <h1 className="text-2xl font-bold text-gray-100 flex items-center gap-2">
            <FolderIcon className="h-6 w-6 text-blue-400" />
            Categories
          </h1>
          <NavLink
            to="/categories/new"
            className="bg-blue-500 hover:bg-blue-400 text-white px-6 py-3 rounded-xl
              flex items-center gap-2 transition-colors"
          >
            <PlusIcon className="h-5 w-5" />
            Add Category
          </NavLink>
        </div>
        <div className="bg-gray-900 rounded-lg shadow-md">
          {categories.length > 0 ? (
            <div className="space-y-4">{renderCategories(categories)}</div>
          ) : (
            <div className="text-center text-gray-400">
              No categories found. Create your first category to organize transactions.
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default CategoryPage;
