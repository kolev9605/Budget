const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const getCurrencies = (token) => {
  return fetch(`${API_BASE_URL}/currencies`, {
    headers: {
      'Authorization': `Bearer ${token}`,
    },
  });
}