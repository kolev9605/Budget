import axiosInstance from "./createAxiosAuth";

export const getCurrencies = async () => {
  const response = await axiosInstance.get('/currencies');
  return response.data;
};