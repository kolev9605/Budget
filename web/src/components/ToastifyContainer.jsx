import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const ToastifyContainer = () => (
  <ToastContainer
    position="top-right"
    autoClose={5000}
    hideProgressBar={false}
    newestOnTop={false}
    closeOnClick
    rtl={false}
    pauseOnFocusLoss
    draggable
    pauseOnHover
    theme="dark"
    toastStyle={{
      backgroundColor: "#1f2937", // Tailwind's gray-800
      color: "#d1d5db", // Tailwind's gray-300
      borderRadius: "0.75rem", // Tailwind's rounded-xl
      boxShadow: "0 4px 6px rgba(0, 0, 0, 0.1)", // Tailwind's shadow-xl
    }}
  />
);

export default ToastifyContainer;
