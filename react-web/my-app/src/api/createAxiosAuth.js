// hooks/useAxiosAuth.ts
import axios from "axios";
import { performLogout } from "../utils/logout";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

const axiosInstance = axios.create({
  baseURL: `${API_BASE_URL}`,
  headers: {
    "Content-Type": "application/json",
  },
});

// Optional: add error handling
axiosInstance.interceptors.response.use(
  (res) => res,
  (error) => {
    if (error.response?.status === 401) {
      performLogout();
    }

    const message = error.response?.data?.message || error.message || "Something went wrong";
    // toast.error(message); // Uncomment if you want global toast here
    return Promise.reject(error);
  }
);

export const setAuthToken = (token) => {
  if (token) {
    axiosInstance.defaults.headers.Authorization = `Bearer ${token}`;
  } else {
    delete axiosInstance.defaults.headers.Authorization;
  }
};

export default axiosInstance;
