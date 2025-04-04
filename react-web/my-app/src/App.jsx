import "./App.css";
import RegisterPage from "./pages/RegisterPage.jsx";
import { Routes, Route, Navigate } from "react-router";
import DashboardPage from "./pages/DashboardPage.jsx";
import AccountsPage from "./pages/AccountsPage.jsx";
import AccountFormPage from "./pages/AccountFormPage.jsx";
import RecordFormPage from "./pages/RecordFormPage.jsx";
import RecordsPage from "./pages/RecordsPage.jsx";
import LoginPage from "./pages/LoginPage.jsx";
import CategoryPage from "./pages/CategoriesPage.jsx";
import CategoryFormPage from "./pages/CategoryFormPage.jsx";
import Layout from "./components/Layout.jsx";
import PrivateRoutes from "./components/PrivateRoutes.jsx";
import UnauthenticatedRoutes from "./components/UnauthenticatedRoutes.jsx";

function App() {
  return (
    <Routes>
      <Route element={<UnauthenticatedRoutes />}>
        <Route path="login" element={<LoginPage />} />
        <Route path="register" element={<RegisterPage />} />
      </Route>
      <Route element={<PrivateRoutes />}>
        <Route element={<Layout />}>
          <Route path="/" element={<DashboardPage />} />
          <Route path="accounts" element={<AccountsPage />}></Route>

          <Route path="accounts/new" element={<AccountFormPage />} />
          <Route path="categories" element={<CategoryPage />} />
          <Route path="categories/new" element={<CategoryFormPage />} />
          <Route path="records" element={<RecordsPage />} />
          <Route path="records/new" element={<RecordFormPage />} />
        </Route>
      </Route>
    </Routes>
  );
}

export default App;
