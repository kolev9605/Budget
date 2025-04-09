export const getCategories = async (axiosAuth) => {
  const response = await axiosAuth.get('/categories');
  return response.data;
}

export const getCategoryById = async (id, axiosAuth) => {
  const response = await axiosAuth.get(`/categories/${id}`);
  return response.data;
}

export const createCategory = async (categoryData, axiosAuth) => {
  const response = await axiosAuth.post('/categories', categoryData);
  return response.data;
}
export const updateCategory = async (categoryData, axiosAuth) => {
  const response = await axiosAuth.put('/categories', categoryData);
  return response.data;
}

export const deleteCategory = async (id, axiosAuth) => {
  const response = await axiosAuth.delete(`/categories/${id}`);
  return response.data;
}

export const getCategoryTypes = async (axiosAuth) => {
  const response = await axiosAuth.get('/categories/types');
  return response.data;
}