import { useState } from "react";
import { useAuthContext } from "./useAuthContext";
import { useNavigate } from "react-router";
import axios from "axios";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const useRegister = () => {
  const [error, setError] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const { dispatch } = useAuthContext();
  const navigate = useNavigate();

  const register = async (email, password) => {
    setError(null);
    setIsLoading(true);

    try {
      const response = await axios.post(`${API_BASE_URL}/authentication/register`, {
        email,
        password,
      });

      const json = response.data;

      localStorage.setItem("user", JSON.stringify(json));
      dispatch({ type: "LOGIN", payload: json });
      setIsLoading(false);

      return navigate("/dashboard");
    } catch (err) {
      setIsLoading(false);
      setError(err.response?.data?.title || "An error occurred");
    }
  };

  return { register, error, isLoading };
};
