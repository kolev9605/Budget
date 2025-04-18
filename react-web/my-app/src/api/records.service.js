import axiosInstance from "./createAxiosAuth";

export const getRecords = async () => {
  const response = await axiosInstance.get("/records");
  return response.data;
}

export const getRecordById = async (id) => {
  const response = await axiosInstance.get(`/records/${id}`);
  return response.data;
}

export const createRecord = async (recordData) => {
  const response = await axiosInstance.post("/records", recordData);
  return response.data;
}

export const updateRecord = async (recordData) => {
  const response = await axiosInstance.put("/records", recordData);
  return response.data;
}

export const deleteRecord = async (id) => {
  const response = await axiosInstance.delete(`/records/${id}`);
  return response.data;
}

export const getRecordTypes = async () => {
  const response = await axiosInstance.get("/records/types");
  return response.data;
}