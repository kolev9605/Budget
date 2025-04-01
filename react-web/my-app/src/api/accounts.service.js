const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export function getAccounts() {
  return fetch(`${API_BASE_URL}/accounts/getall`)
    .then((response) => response.json())
    .catch((error) => console.error('Error fetching accounts:', error));
}