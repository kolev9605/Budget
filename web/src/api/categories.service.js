import axiosInstance from "./createAxiosAuth";

export const getCategories = async (primaryOnly) => {
  const response = await axiosInstance.get("/categories", {
    params: { primaryOnly },
  });

  return response.data;
};

export const getCategoryById = async (id) => {
  const response = await axiosInstance.get(`/categories/${id}`);
  return response.data;
};

export const createCategory = async (categoryData) => {
  const response = await axiosInstance.post("/categories", categoryData);
  return response.data;
};
export const updateCategory = async (categoryData) => {
  const response = await axiosInstance.put("/categories", categoryData);
  return response.data;
};

export const deleteCategory = async (id) => {
  const response = await axiosInstance.delete(`/categories/${id}`);
  return response.data;
};

export const getCategoryTypes = async () => {
  const response = await axiosInstance.get("/categories/types");
  return response.data;
};

export const getMostUsedCategories = async () => {
  const response = await axiosInstance.get("/categories/most-used");
  return response.data;
}
