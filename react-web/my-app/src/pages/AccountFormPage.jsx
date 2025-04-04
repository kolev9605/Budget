import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { WalletIcon, CurrencyDollarIcon, XMarkIcon, ChevronLeftIcon } from "@heroicons/react/24/outline";
import { getCurrencies } from "../api/currencies.service";
import { createAccount } from "../api/accounts.service.js";
import ErrorSection from "../components/ErrorSection.jsx";
import { useAuthContext } from "../hooks/useAuthContext.js";

const AccountFormPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { user } = useAuthContext();
  const isEditing = !!id;

  const [formData, setFormData] = useState({
    name: "",
    balance: "",
    currencyId: "",
    description: "",
  });

  const [errors, setErrors] = useState({});
  const [currencies, setCurrencies] = useState([]);

  useEffect(() => {
    // if (isEditing) {
    //   // Replace with actual data fetching
    //   const fakeAccountData = {
    //     name: "Example Account",
    //     balance: 1500.0,
    //     currency: "USD",
    //     description: "Sample account data",
    //   };
    // }

    const fetchCurrencies = async () => {
      const response = await getCurrencies(user.token);
      if (!response.ok) {
        // TODO: Toastify error
        return;
      }

      const data = await response.json();
      setCurrencies(data);
      setFormData((prevFormData) => ({
        ...prevFormData,
        currencyId: data[0]?.id || "",
      }));
    };

    if (user) fetchCurrencies();
  }, [isEditing, user]);

  const validateForm = () => {
    const newErrors = {};

    if (!formData.name.trim()) newErrors.name = "Account name is required";
    if (isNaN(formData.balance)) newErrors.balance = "Valid balance is required";
    if (!formData.currencyId.trim()) newErrors.currency = "Currency is required";

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

    const submitCreateAccount = async () => {
      const response = await createAccount(accountData, user.token);

      if (!response.ok) {
        // todo: toast
        return;
      }
    };

    submitCreateAccount();

    if (isEditing) {
      accountData.id = id;
    }

    navigate("/accounts");
  };

  return (
    <div className="min-h-screen bg-gray-900 p-6 sm:p-8 lg:p-10">
      <div className="max-w-2xl mx-auto">
        <div className="bg-gray-800 p-6 rounded-2xl shadow-xl">
          <div className="flex items-center mb-6">
            <button
              onClick={() => navigate(-1)}
              className="flex items-center text-gray-400 hover:text-blue-400 px-3 py-2 rounded-lg transition-colors bg-gray-800 hover:bg-gray-700 shadow-md"
            >
              <ChevronLeftIcon className="h-5 w-5" />
            </button>
            <h1 className="text-2xl font-bold text-gray-100 ml-2">
              {isEditing ? "Edit Account" : "Create New Account"}
            </h1>
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
                  value={formData.currencyId}
                  onChange={(e) => setFormData({ ...formData, currencyId: e.target.value })}
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
