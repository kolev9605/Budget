import "./App.css";
import RegisterPage from "./pages/RegisterPage.jsx";
import Navbar from "./components/Navbar.jsx";
import { BrowserRouter, Routes, Route } from "react-router";
import DashboardPage from "./pages/DashboardPage.jsx";
import AccountsPage from "./pages/AccountsPage.jsx";
import AccountFormPage from "./pages/AccountFormPage.jsx";
import RecordFormPage from "./pages/RecordFormPage.jsx";
import RecordsPage from "./pages/RecordsPage.jsx";
import RecordsPagev2 from "./pages/RecordsPagev2.jsx";
import LoginPage from "./pages/LoginPage.jsx";
import CategoryPage from "./pages/CategoriesPage.jsx";
import CategoryFormPage from "./pages/CategoryFormPage.jsx";

function App() {
  return (
    <Routes>
      <Route path="/" element={<App />} />
      <Route path="login" element={<LoginPage/>} />
      <Route path="register" element={<RegisterPage />} />
      <Route path="nav" element={<Navbar />} />
      <Route path="dashboard" element={<DashboardPage />} />
      <Route path="accounts" element={<AccountsPage />} />
      <Route path="addaccount" element={<AccountFormPage />} />
      <Route path="add2" element={<RecordFormPage />} />
      <Route path="records" element={<RecordsPage />} />
      <Route path="categories" element={<CategoryPage/>} />
      <Route path="categoryform" element={<CategoryFormPage/>} />
      <Route path="records2" element={<RecordsPagev2 />} />
    </Routes>
  );
}

export default App;
