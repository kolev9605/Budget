import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import { BrowserRouter } from "react-router";
import App from "./App";
import { AuthContextProvider } from "./context/AuthContext";
import ToastifyContainer from "./components/ToastifyContainer";

createRoot(document.getElementById("root")).render(
  // <StrictMode>
  <AuthContextProvider>
    <BrowserRouter>
      <ToastifyContainer />
      <App />
    </BrowserRouter>
  </AuthContextProvider>
  // </StrictMode>
);
