import { useAuthContext } from "./useAuthContext";
import { useNavigate } from "react-router";

export const useLogout = () => {
  const { dispatch } = useAuthContext();
  const navigate = useNavigate();

  const logout = () => {
    localStorage.removeItem("user");

    dispatch({ type: "LOGOUT" });
    return navigate("/login");
  };

  return { logout };
};
