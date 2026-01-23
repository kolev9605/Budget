import { Navigate, Outlet } from "react-router-dom";
import { useAuthContext } from "../hooks/useAuthContext.js";

const PrivateRoutes = () => {
  const { user, loading } = useAuthContext();

  if (loading) return null;

  return user?.token ? <Outlet /> : <Navigate to="/login" />;
};

export default PrivateRoutes;
