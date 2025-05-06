import { Navigate, Outlet } from "react-router-dom";
import { useAuthContext } from "../hooks/useAuthContext.js";

const UnauthenticatedRoutes= () => {
  const { user, loading } = useAuthContext();

  if (loading) return null;

  return !user?.token ? <Outlet /> : <Navigate to="/" />;
};

export default UnauthenticatedRoutes;