export const getCategories = async (axiosAuth) => {
  const response = await axiosAuth.get('/categories');
  return response.data;
}