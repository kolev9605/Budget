const LoadingOverlay = () => {
  return (
    <div className="min-h-screen bg-gray-900 p-6 sm:p-8 lg:p-10">
      <div className="fixed inset-0 backdrop-blur-xs bg-black/20 flex items-center justify-center z-50">
        <div className="w-16 h-16 border-4 border-white border-t-transparent rounded-full animate-spin"></div>
      </div>
    </div>
  );
};

export default LoadingOverlay;
