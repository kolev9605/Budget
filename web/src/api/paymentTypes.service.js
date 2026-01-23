import axiosInstance from "./createAxiosAuth";

export const getPaymentTypes = async () => {
  const response = await axiosInstance.get("/paymenttypes");
  return response.data;
}