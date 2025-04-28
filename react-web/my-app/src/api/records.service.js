import axiosInstance from "./createAxiosAuth";

export const getRecords = async (
  pageNumber,
  pageSize,
  accountId,
  startDateRange,
  endDateRange,
  recordType,
  categoryId
) => {
  const response = await axiosInstance.get("/records", {
    params: {
      pageNumber: pageNumber,
      pageSize: pageSize,
      accountId: accountId === "all" ? null : accountId,
      startDateRange: startDateRange,
      endDateRange: endDateRange,
      recordType: recordType === "all" ? null : recordType,
      categoryId: categoryId === "all" ? null : categoryId,
    },
  });
  return response.data;
};

export const getRecordById = async (id) => {
  const response = await axiosInstance.get(`/records/${id}`);
  return response.data;
};

export const createRecord = async (recordData) => {
  const response = await axiosInstance.post("/records", recordData);
  return response.data;
};

export const updateRecord = async (recordData) => {
  const response = await axiosInstance.put("/records", recordData);
  return response.data;
};

export const deleteRecord = async (id) => {
  const response = await axiosInstance.delete(`/records/${id}`);
  return response.data;
};

export const getRecordTypes = async () => {
  const response = await axiosInstance.get("/records/types");
  return response.data;
};

export const getStatistics = async (startDateRange, endDateRange) => {
  const response = await axiosInstance.get("/records/statistics", {
    params: {
      startDateRange: startDateRange,
      endDateRange: endDateRange,
    },
  });
  return response.data;
};

export const getTotalBalance = async () => {
  const response = await axiosInstance.get("/records/totalbalance");
  return response.data;
};
