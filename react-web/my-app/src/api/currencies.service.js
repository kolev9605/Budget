const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const getCurrencies = () => {
  return fetch(`${API_BASE_URL}/currencies`);
}