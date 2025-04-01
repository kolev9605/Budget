import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.jsx";
import LoginForm from "./components/LoginForm.jsx";
import RegisterForm from "./components/RegisterForm.jsx";
import Navbar from "./components/Navbar.jsx";
import { BrowserRouter, Routes, Route } from "react-router";
import Dashboard from "./components/Dashboard.jsx";
import AccountsPage from "./components/AccountsPage.jsx";
import AccountFormPage from "./components/AccountFormPage.jsx";
import RecordFormPage from "./components/RecordFormPage.jsx";
import RecordsPage from "./components/RecordsPage.jsx";
import RecordsPagev2 from "./components/RecordsPagev2.jsx";

createRoot(document.getElementById("root")).render(
  <StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<App />} />
        <Route path="login" element={<LoginForm />} />
        <Route path="register" element={<RegisterForm />} />
        <Route path="nav" element={<Navbar />} />
        <Route path="dashboard" element={<Dashboard />} />
        <Route path="accounts" element={<AccountsPage />} />
        <Route path="addaccount" element={<AccountFormPage />} />
        <Route path="add2" element={<RecordFormPage />} />
        <Route path="records" element={<RecordsPage />} />
        <Route path="records2" element={<RecordsPagev2 />} />
      </Routes>
    </BrowserRouter>
  </StrictMode>
);
