import "./App.css";
import RegisterPage from "./pages/RegisterPage.jsx";
import Navbar from "./components/Navbar.jsx";
import { BrowserRouter, Routes, Route } from "react-router";
import DashboardPage from "./pages/DashboardPage.jsx";
import AccountsPage from "./pages/AccountsPage.jsx";
import AccountFormPage from "./pages/AccountFormPage.jsx";
import RecordFormPage from "./pages/RecordFormPage.jsx";
import RecordsPageOld from "./pages/RecordsPageOld.jsx";
import RecordsPage from "./pages/RecordsPage.jsx";
import LoginPage from "./pages/LoginPage.jsx";
import CategoryPage from "./pages/CategoriesPage.jsx";
import CategoryFormPage from "./pages/CategoryFormPage.jsx";
import Layout from "./components/Layout.jsx";

function App() {
  return (
    <Routes>
      <Route path="login" element={<LoginPage />} />
      <Route path="register" element={<RegisterPage />} />
      <Route element={<Layout />}>
        <Route path="dashboard" element={<DashboardPage />} />
        <Route path="accounts" element={<AccountsPage />}></Route>

        <Route path="accounts/new" element={<AccountFormPage />} />
        <Route path="categories" element={<CategoryPage />} />
        <Route path="categories/new" element={<CategoryFormPage />} />
        <Route path="records" element={<RecordsPage />} />
        <Route path="records/new" element={<RecordFormPage />} />
      </Route>
    </Routes>
  );
}

export default App;
