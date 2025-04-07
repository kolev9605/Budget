// hooks/useAxiosAuth.ts
import axios from "axios";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const createAxiosAuth = (token) => {

  const axiosInstance = axios.create({
    baseURL: `${API_BASE_URL}`,
    headers: {
      "Content-Type": "application/json",
      ...(token && { Authorization: `Bearer ${token}` }),
    },
  });

  // Optional: add error handling
  axiosInstance.interceptors.response.use(
    (res) => res,
    (error) => {
      const message =
        error.response?.data?.message ||
        error.message ||
        "Something went wrong";
      // toast.error(message); // Uncomment if you want global toast here
      return Promise.reject(error);
    }
  );

  return axiosInstance;
};
