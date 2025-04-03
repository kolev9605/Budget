import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
  BanknotesIcon,
  CreditCardIcon,
  WalletIcon,
  CurrencyDollarIcon,
  XMarkIcon,
  InformationCircleIcon,
} from "@heroicons/react/24/outline";
import { getCurrencies } from "../api/currencies.service";
import ErrorSection from "../components/ErrorSection.jsx";

const AccountFormPage = ({ existingAccounts = [] }) => {
  const { id } = useParams();
  const navigate = useNavigate();
  const isEditing = !!id;

  // Sample account types - extend as needed
  const accountTypes = [
    { value: "cash", label: "Cash", icon: WalletIcon },
    { value: "credit", label: "Credit Card", icon: CreditCardIcon },
    { value: "debit", label: "Debit Card", icon: CreditCardIcon },
    { value: "savings", label: "Savings Account", icon: BanknotesIcon },
    { value: "investment", label: "Investment", icon: BanknotesIcon },
  ];

  const [formData, setFormData] = useState({
    name: "",
    type: "cash",
    balance: "",
    currency: "",
    description: "",
  });

  const [errors, setErrors] = useState({});
  const [currencies, setCurrencies] = useState([]);

  // Load data if editing
  useEffect(() => {
    if (isEditing) {
      // Replace with actual data fetching
      const fakeAccountData = {
        name: "Example Account",
        type: "credit",
        balance: 1500.0,
        currency: "USD",
        description: "Sample account data",
      };
    }

    const fetchCurrencies = async () => {
      try {
        const response = await getCurrencies();
        if (!response.ok) {
          // TODO: Toastify error
          throw new Error("Failed to fetch currencies");
        }
        const data = await response.json();
        setCurrencies(data);
        setFormData((prevFormData) => ({
          ...prevFormData,
          currency: data[0]?.id || "",
        }));
      } catch (error) {
        // TODO: Toastify error
        console.error("Error fetching currencies:", error);
      }
    };

    fetchCurrencies();
  }, [isEditing]);

  const validateForm = () => {
    const newErrors = {};

    if (!formData.name.trim()) newErrors.name = "Account name is required";
    if (!formData.type) newErrors.type = "Account type is required";
    if (isNaN(formData.balance)) newErrors.balance = "Valid balance is required";

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!validateForm()) return;

    const accountData = {
      ...formData,
      balance: parseFloat(formData.balance),
    };

    if (isEditing) {
      accountData.id = id;
    }

    console.log("Saving account:", accountData);
    // navigate("/accounts");
  };

  return (
    <div className="min-h-screen bg-gray-900 p-6 sm:p-8 lg:p-10">
      <div className="max-w-2xl mx-auto">
        <div className="bg-gray-800 p-6 rounded-2xl shadow-xl">
          <div className="flex items-center justify-between mb-6">
            <h1 className="text-2xl font-bold text-gray-100">{isEditing ? "Edit Account" : "Create New Account"}</h1>
            <button
              onClick={() => navigate(-1)}
              className="text-gray-400 hover:text-blue-400 p-2 rounded-lg transition-colors"
            >
              <XMarkIcon className="h-6 w-6" />
            </button>
          </div>

          <ErrorSection errors={errors} />

          <form onSubmit={handleSubmit} className="space-y-6">
            {/* Account Name */}
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-3">Account Name</label>
              <div className="relative">
                <input
                  type="text"
                  value={formData.name}
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                  className="w-full bg-gray-700 border border-gray-600 rounded-xl pl-12 pr-4 py-3.5
                    text-gray-100 placeholder-gray-500 focus:outline-none focus:border-blue-400
                    focus:ring-2 focus:ring-blue-400/30 transition-all"
                  placeholder="e.g. Primary Credit Card"
                />
                <WalletIcon className="h-5 w-5 text-gray-400 absolute left-4 top-1/2 -translate-y-1/2" />
              </div>
            </div>

            {/* Account Type */}
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-3">Account Type</label>
              <div className="grid grid-cols-2 md:grid-cols-3 gap-4">
                {accountTypes.map((type) => {
                  const Icon = type.icon;
                  return (
                    <button
                      key={type.value}
                      type="button"
                      onClick={() => setFormData({ ...formData, type: type.value })}
                      className={`p-4 rounded-xl flex flex-col items-center gap-2 transition-colors
                        ${
                          formData.type === type.value
                            ? "bg-blue-500 text-white"
                            : "bg-gray-700 hover:bg-gray-600 text-gray-300"
                        }
                      `}
                    >
                      <Icon className="h-6 w-6" />
                      <span className="text-sm">{type.label}</span>
                    </button>
                  );
                })}
              </div>
            </div>

            {/* Balance and Currency */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label className="block text-sm font-medium text-gray-300 mb-3">Initial Balance</label>
                <div className="relative">
                  <input
                    type="number"
                    step="0.01"
                    value={formData.balance}
                    onChange={(e) => setFormData({ ...formData, balance: e.target.value })}
                    className="w-full bg-gray-700 border border-gray-600 rounded-xl pl-12 pr-4 py-3.5
                      text-gray-100 placeholder-gray-500 focus:outline-none focus:border-blue-400
                      focus:ring-2 focus:ring-blue-400/30 transition-all"
                    placeholder="0.00"
                  />
                  <CurrencyDollarIcon className="h-5 w-5 text-gray-400 absolute left-4 top-1/2 -translate-y-1/2" />
                </div>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-300 mb-3">Currency</label>
                <select
                  value={formData.currency}
                  onChange={(e) => setFormData({ ...formData, currency: e.target.value })}
                  className="w-full bg-gray-700 border border-gray-600 rounded-xl px-4 py-3.5
                    text-gray-100 focus:outline-none focus:border-blue-400 focus:ring-2 
                    focus:ring-blue-400/30 appearance-none"
                >
                  {currencies.map((c) => (
                    <option key={c.id} value={c.id}>
                      {c.abbreviation}
                    </option>
                  ))}
                </select>
              </div>
            </div>

            {/* Description */}
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-3">Description (Optional)</label>
              <textarea
                value={formData.description}
                onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                className="w-full bg-gray-700 border border-gray-600 rounded-xl px-4 py-3.5
                  text-gray-100 placeholder-gray-500 focus:outline-none focus:border-blue-400
                  focus:ring-2 focus:ring-blue-400/30 transition-all resize-none"
                placeholder="Add account details or notes..."
                rows="3"
              />
            </div>

            {/* Form Actions */}
            <div className="flex flex-col-reverse sm:flex-row gap-4 mt-8">
              <button
                type="button"
                onClick={() => navigate(-1)}
                className="w-full sm:w-auto px-6 py-3 text-gray-300 hover:text-white 
                  bg-gray-700 hover:bg-gray-600 rounded-xl transition-colors"
              >
                Cancel
              </button>
              <button
                type="submit"
                className="w-full sm:w-auto px-6 py-3 bg-blue-500 hover:bg-blue-400 
                  text-white rounded-xl flex items-center justify-center gap-2 transition-colors"
              >
                {isEditing ? "Save Changes" : "Create Account"}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default AccountFormPage;
