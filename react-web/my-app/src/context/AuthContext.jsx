import { createContext, useEffect, useReducer, useState } from "react";
import { setAuthToken } from "../api/createAxiosAuth";

export const AuthContext = createContext();

export const authReducer = (state, action) => {
  switch (action.type) {
    case "LOGIN":
      console.log("Login action:", action);

      return { user: action.payload };
    case "LOGOUT":
      return { user: null };
    default:
      return state;
  }
};

export const AuthContextProvider = ({ children }) => {
  const [state, dispatch] = useReducer(authReducer, {
    user: null,
  });

  const [loading, setLoading] = useState(true); // Add loading state

  useEffect(() => {
    const user = JSON.parse(localStorage.getItem("user"));
    if (user) {
      dispatch({ type: "LOGIN", payload: user });
      setAuthToken(user.token);
    }

    setLoading(false); // Set loading to false after resolving user
  }, []);

  console.log("AuthContext state:", state);

  return <AuthContext.Provider value={{ ...state, dispatch, loading }}>{children}</AuthContext.Provider>;
};
