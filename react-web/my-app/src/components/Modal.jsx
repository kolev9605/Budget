import React, { useEffect } from "react";

const Modal = ({ children, onClose }) => {
  useEffect(() => {
    // Disable scrolling when the modal is open
    document.body.style.overflow = "hidden";
    return () => {
      // Re-enable scrolling when the modal is closed
      document.body.style.overflow = "auto";
    };
  }, []);

  return (
    <div
      className="fixed inset-0 backdrop-blur-sm bg-opacity-0 flex items-center justify-center z-50"
      onClick={onClose} // Close modal when clicking on the backdrop
    >
      <div
        className="bg-gray-800 rounded-lg shadow-lg p-6 w-auto max-w-3xl relative overflow-auto max-h-[90vh]"
        onClick={(e) => e.stopPropagation()} // Prevent closing when clicking inside the modal
      >
        <button
          onClick={onClose}
          className="absolute top-3 right-3 text-gray-400 hover:text-gray-300"
        >
          ✕
        </button>
        {children}
      </div>
    </div>
  );
};

export default Modal;
