import "./App.css";
import RegisterPage from "./pages/RegisterPage.jsx";
import { Routes, Route, Navigate } from "react-router";
import DashboardPage from "./pages/DashboardPage.jsx";
import AccountsPage from "./pages/accounts/AccountsPage.jsx";
import RecordFormPage from "./pages/records/RecordFormPage.jsx";
import RecordsPage from "./pages/records/RecordsPage.jsx";
import LoginPage from "./pages/LoginPage.jsx";
import CategoryPage from "./pages/categories/CategoriesPage.jsx";
import Layout from "./components/Layout.jsx";
import PrivateRoutes from "./components/PrivateRoutes.jsx";
import UnauthenticatedRoutes from "./components/UnauthenticatedRoutes.jsx";
import AddAccountPage from "./pages/accounts/AddAccountPage.jsx";
import EditAccountPage from "./pages/accounts/EditAccountPage.jsx";
import AddCategoryPage from "./pages/categories/AddCategoryPage.jsx";
import EditCategoryPage from "./pages/categories/EditCategoryPage.jsx";

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

          <Route path="accounts/new" element={<AddAccountPage/>} />
          <Route path="accounts/edit/:id" element={<EditAccountPage />} />
          <Route path="categories" element={<CategoryPage />} />
          <Route path="categories/new" element={<AddCategoryPage/>} />
          <Route path="categories/edit/:id" element={<EditCategoryPage/>} />
          <Route path="records" element={<RecordsPage />} />
          <Route path="records/new" element={<RecordFormPage />} />
        </Route>
      </Route>
    </Routes>
  );
}

export default App;
