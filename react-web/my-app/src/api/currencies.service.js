export const getCurrencies = async (axiosAuth) => {
  const response = await axiosAuth.get('/currencies');
  return response.data;
};