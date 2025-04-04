const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const getAccounts = (token) => {
  return fetch(`${API_BASE_URL}/accounts`, {
    headers: {
      'Authorization': `Bearer ${token}`,
    },
  });
}

export const getAccountById = (id) => {
  return fetch(`${API_BASE_URL}/accounts/${id}`)
}

export const createAccount = (account, token) => {
  return fetch(`${API_BASE_URL}/accounts`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`,
    },
    body: JSON.stringify(account),
  });
}

export const updateAccount = (id, account) => {
  return fetch(`${API_BASE_URL}/accounts/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(account),
  });
}

export const deleteAccount = (id) => {
  return fetch(`${API_BASE_URL}/accounts/${id}`, {
    method: 'DELETE',
  });
}