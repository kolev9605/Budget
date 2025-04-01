import { useState } from "react";
import { useAuthContext } from "./useAuthContext";
import { useNavigate } from "react-router";

export const useRegister = () => {
  const [error, setError] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const { dispatch } = useAuthContext();
  const navigate = useNavigate();

  const register = async (email, password) => {
    setError(null);
    setIsLoading(true);

    const response = await fetch("http://localhost:5000/authentication/register", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email, password }),
    });

    const json = await response.json();

    if (!response.ok) {
      setIsLoading(false);
      console.log(json.title);
      
      setError(json.title);
    }

    if (response.ok) {
      // save the user to local storage
      console.log("User registered in:", json);

      localStorage.setItem("user", JSON.stringify(json));

      // update the auth context
      dispatch({ type: "LOGIN", payload: json });

      setIsLoading(false);
      
      return navigate("/dashboard");
    }
  };

  return { register, error, isLoading };
};
