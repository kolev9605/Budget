export const getRecords = async (axiosAuth) => {
  const response = await axiosAuth.get("/records");
  return response.data;
}

export const getRecordById = async (id, axiosAuth) => {
  const response = await axiosAuth.get(`/records/${id}`);
  return response.data;
}

export const createRecord = async (recordData, axiosAuth) => {
  const response = await axiosAuth.post("/records", recordData);
  return response.data;
}

export const updateRecord = async (recordData, axiosAuth) => {
  const response = await axiosAuth.put("/records", recordData);
  return response.data;
}

export const deleteRecord = async (id, axiosAuth) => {
  const response = await axiosAuth.delete(`/records/${id}`);
  return response.data;
}

export const getRecordTypes = async (axiosAuth) => {
  const response = await axiosAuth.get("/records/types");
  return response.data;
}