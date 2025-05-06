import { useState } from "react";
import { FiMenu, FiX } from "react-icons/fi";
import { useLogout } from "../hooks/useLogout";
import { NavLink } from "react-router";

const Navbar = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const { logout } = useLogout();
  const handleLogout = () => {
    logout();
  };

  const navItems = [
    { name: "Dashboard", to: "/" },
    { name: "Records", to: "/records" },
    { name: "Accounts", to: "/accounts" },
    { name: "Categories", to: "/categories" },
  ];

  return (
    <nav className="bg-gray-800 shadow-xl">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-16">
          {/* Logo */}
          <div className="flex-shrink-0">
            <span className="text-2xl font-bold text-blue-400">Budget</span>
          </div>

          {/* Desktop Menu */}
          <div className="hidden md:block">
            <div className="ml-10 flex items-baseline space-x-8">
              {navItems.map((item) => (
                <NavLink
                  key={item.name}
                  to={item.to}
                  className="text-gray-300 hover:text-blue-300 px-3 py-2 rounded-md text-sm font-medium transition-colors duration-200"
                >
                  {item.name}
                </NavLink>
              ))}
              <button
                onClick={handleLogout}
                className="ml-4 bg-blue-500 hover:bg-blue-400 text-white px-6 py-2 rounded-xl text-sm font-medium transition-colors duration-200"
              >
                Logout
              </button>
            </div>
          </div>

          {/* Mobile Menu Button */}
          <div className="md:hidden">
            <button
              onClick={() => setIsMenuOpen(!isMenuOpen)}
              className="text-gray-300 hover:text-white focus:outline-none focus:ring-2 focus:ring-blue-400"
            >
              {isMenuOpen ? <FiX className="h-6 w-6" /> : <FiMenu className="h-6 w-6" />}
            </button>
          </div>
        </div>
      </div>

      {/* Mobile Menu */}
      {isMenuOpen && (
        <div className="md:hidden absolute w-full bg-gray-800 shadow-xl z-50">
          <div className="px-2 pt-2 pb-3 space-y-1 sm:px-3">
            {navItems.map((item) => (
              <NavLink
                key={item.name}
                to={item.to}
                className="text-gray-300 hover:bg-gray-700 hover:text-white block px-3 py-2 rounded-md text-base font-medium transition-colors duration-200"
              >
                {item.name}
              </NavLink>
            ))}
            <button
              onClick={handleLogout}
              className="w-full bg-blue-500 hover:bg-blue-400 text-white px-6 py-3 rounded-xl text-base font-medium transition-colors duration-200 mt-2"
            >
              Logout
            </button>
          </div>
        </div>
      )}
    </nav>
  );
};

export default Navbar;
