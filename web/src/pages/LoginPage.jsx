import { useState } from "react";
import { FaUser, FaLock } from "react-icons/fa";
import { InformationCircleIcon } from "@heroicons/react/24/outline";
import { NavLink } from "react-router";
import { useLogin } from "../hooks/useLogin";
import LoadingOverlay from "../components/LoadingOverlay";

const LoginPage = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [rememberMe, setRememberMe] = useState(false);
  const { login, error, isLoading } = useLogin();

  const handleSubmit = async (e) => {
    e.preventDefault();
    await login(email, password);
  };

  return isLoading ? (
    <LoadingOverlay />
  ) : (
    <div className="min-h-screen bg-gray-900 flex items-center justify-center p-4 sm:p-6 lg:p-8">
      <div className="bg-gray-800 p-8 rounded-2xl shadow-xl w-full max-w-md transition-all duration-300 hover:shadow-2xl">
        <div className="mb-10 text-center space-y-3">
          <h1 className="text-3xl md:text-4xl font-bold text-gray-100 transition-all duration-300">Welcome Back</h1>
          <p className="text-gray-400 text-sm md:text-base">Sign in to your account {isLoading}</p>
        </div>

        {error && (
          <div className="bg-red-500/20 border border-red-500/30 rounded-xl p-4 mb-6">
            <div className="flex items-start gap-2 text-red-400">
              <InformationCircleIcon className="h-5 w-5 flex-shrink-0" />
              <div className="space-y-1">
                <p className="text-sm font-medium">There were error(s) with your submission</p>
                <ul className="list-disc list-inside text-sm">
                  <li> {error}</li>
                </ul>
              </div>
            </div>
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-8">
          <div className="space-y-6">
            <div>
              <label htmlFor="email" className="block text-sm font-medium text-gray-300 mb-3">
                Email Address
              </label>
              <div className="relative">
                <FaUser className="absolute top-1/2 left-4 transform -translate-y-1/2 text-gray-500" />
                <input
                  type="email"
                  id="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  className="w-full pl-12 pr-4 py-3.5 bg-gray-700 border border-gray-600 rounded-xl 
                    focus:outline-none focus:border-blue-400 focus:ring-2 focus:ring-blue-400/30
                    text-gray-100 placeholder-gray-500 transition-all duration-200"
                  placeholder="name@company.com"
                  required
                />
              </div>
            </div>
            <div>
              <label htmlFor="password" className="block text-sm font-medium text-gray-300 mb-3">
                Password
              </label>
              <div className="relative">
                <FaLock className="absolute top-1/2 left-4 transform -translate-y-1/2 text-gray-500" />
                <input
                  type="password"
                  id="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  className="w-full pl-12 pr-4 py-3.5 bg-gray-700 border border-gray-600 rounded-xl 
                    focus:outline-none focus:border-blue-400 focus:ring-2 focus:ring-blue-400/30
                    text-gray-100 placeholder-gray-500 transition-all duration-200"
                  placeholder="••••••••"
                  required
                />
              </div>
            </div>
          </div>

          <div className="flex items-center justify-between">
            <label className="flex items-center space-x-3 cursor-pointer">
              <input
                type="checkbox"
                checked={rememberMe}
                onChange={(e) => setRememberMe(e.target.checked)}
                className="h-4 w-4 text-blue-400 focus:ring-blue-400/30 bg-gray-700 border-gray-600 rounded"
              />
              <span className="text-sm text-gray-400">Remember me</span>
            </label>
            <a href="#" className="text-sm text-blue-400 hover:text-blue-300 transition-colors duration-200">
              Forgot password?
            </a>
          </div>

          <button
            disabled={isLoading}
            type="submit"
            className={`w-full py-3.5 rounded-xl font-medium text-sm md:text-base tracking-wide transition-all duration-300 transform shadow-lg
              ${
                isLoading
                  ? "bg-gray-500 text-gray-300 cursor-not-allowed"
                  : "bg-blue-500 hover:bg-blue-400 text-white hover:scale-[1.01] hover:shadow-blue-500/20"
              }`}
          >
            Sign In
          </button>
        </form>

        <p className="mt-8 text-center text-sm text-gray-400">
          Don't have an account?{" "}
          <NavLink
            to="/register"
            className="text-blue-400 hover:text-blue-300 font-medium transition-colors duration-200"
          >
            Get started
          </NavLink>
        </p>
      </div>
    </div>
  );
};

export default LoginPage;
