import axiosInstance from "./createAxiosAuth";

export const getCashFlow = async (startDateRange, endDateRange) => {
  const response = await axiosInstance.get("/statistics/cashflow", {
    params: {
      startDateRange: startDateRange,
      endDateRange: endDateRange,
    },
  });
  return response.data;
};
