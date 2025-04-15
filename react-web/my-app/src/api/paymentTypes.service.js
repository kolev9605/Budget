export const getPaymentTypes = async (axiosAuth) => {
  const response = await axiosAuth.get("/paymenttypes");
  return response.data;
}