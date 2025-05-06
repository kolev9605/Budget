import { useAuthContext } from "./useAuthContext";
import { performLogout } from "../utils/logout";

export const useLogout = () => {
  const { dispatch } = useAuthContext();

  const logout = () => {
    performLogout();
    dispatch({ type: "LOGOUT" });
  };

  return { logout };
};
