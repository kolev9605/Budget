export const getAccounts = async (axiosAuth) => {
  const response = await axiosAuth.get('/accounts');
  return response.data;
};

export const getAccountById = async (id, axiosAuth) => {
  const response = await axiosAuth.get(`/accounts/${id}`);
  return response.data;
};

export const createAccount = async (account, axiosAuth) => {
  const response = await axiosAuth.post('/accounts', account);
  return response.data;
};

export const updateAccount = async (account, axiosAuth) => {
  const response = await axiosAuth.put('/accounts', account);
  return response.data;
};

export const deleteAccount = async (id, axiosAuth) => {
  const response = await axiosAuth.delete(`/accounts/${id}`);
  return response.data;
};