import axiosInstance from "./createAxiosAuth"; // Adjust the import path as necessary

export const getAccounts = async (includeHidden = false) => {
  const response = await axiosInstance.get("/accounts",
    {
      params: {
        includeHidden,
      },
    }
  );
  return response.data;
};

export const getAccountById = async (id) => {
  const response = await axiosInstance.get(`/accounts/${id}`);
  return response.data;
};

export const createAccount = async (account) => {
  const response = await axiosInstance.post("/accounts", account);
  return response.data;
};

export const updateAccount = async (account) => {
  const response = await axiosInstance.put("/accounts", account);
  return response.data;
};

export const deleteAccount = async (id) => {
  const response = await axiosInstance.delete(`/accounts/${id}`);
  return response.data;
};

export const toggleAccountActivationStatus = async (id) => {
  const response = await axiosInstance.post(`/accounts/${id}/toggle-activation`);
  return response.data;
};