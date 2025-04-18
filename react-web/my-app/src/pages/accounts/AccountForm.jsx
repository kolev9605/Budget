import { useState, useEffect } from "react";
import { WalletIcon, CurrencyDollarIcon } from "@heroicons/react/24/outline";
import ErrorSection from "../../components/ErrorSection.jsx";
import { getCurrencies } from "../../api/currencies.service.js";
import { getPaymentTypes } from "../../api/paymentTypes.service.js";

const AccountForm = ({ account, onSubmit }) => {
  const [formData, setFormData] = useState(account);
  const [errors, setErrors] = useState({});
  const [currencies, setCurrencies] = useState([]);
  const [paymentTypes, setPaymentTypes] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      const currencies = await getCurrencies();
      setCurrencies(currencies);

      const paymentTypes = await getPaymentTypes();
      setPaymentTypes(paymentTypes);

      setFormData((prevFormData) => ({
        ...prevFormData,
        currencyId: account?.currency?.id || currencies[0]?.id || "",
        paymentTypeId: account?.paymentType?.id || paymentTypes[0]?.id || "",
      }));
    };

    fetchData();
  }, [account]);

  const validateForm = () => {
    const newErrors = {};

    if (!formData.name.trim()) newErrors.name = "Account name is required";
    if (isNaN(formData.initialBalance)) newErrors.initialBalance = "Valid initial balance is required";
    if (!formData.currencyId.trim()) newErrors.currency = "Currency is required";

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!validateForm()) return;

    onSubmit({
      ...formData,
      initialBalance: parseFloat(formData.initialBalance),
    });
  };

  return (
    <div className="min-h-screen bg-gray-900 p-6 sm:p-8 lg:p-10">
      <div className="max-w-2xl mx-auto">
        <div className="bg-gray-800 p-6 rounded-2xl shadow-xl">
          <h1 className="text-2xl font-bold text-gray-100 mb-6">{account?.id ? "Edit Account" : "Create New Account"}</h1>
          <form onSubmit={handleSubmit} className="space-y-6">
            <ErrorSection errors={errors} />

            {/* Account Name */}
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-3">Name</label>
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

            <div>
              <label className="block text-sm font-medium text-gray-300 mb-3">Payment Type</label>
              <div className="grid grid-cols-2 sm:grid-cols-4 gap-4">
                {paymentTypes.map((paymentType) => (
                  <div
                    key={paymentType.id}
                    onClick={() => setFormData({ ...formData, paymentTypeId: paymentType.id })}
                    className={`cursor-pointer p-4 rounded-lg shadow-md transition-transform transform flex flex-col items-center justify-center gap-2 ${
                      formData.paymentTypeId === paymentType.id
                        ? "bg-blue-500 text-white scale-105"
                        : "bg-gray-800 text-gray-300 hover:bg-gray-700 hover:scale-105"
                    }`}
                  >
                    <span className="text-sm font-bold">{paymentType.name}</span>
                  </div>
                ))}
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
                    value={formData.initialBalance}
                    onChange={(e) => setFormData({ ...formData, initialBalance: e.target.value })}
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

            {/* Form Actions */}
            <div className="flex flex-col-reverse sm:flex-row gap-4 mt-8">
              <button
                type="button"
                onClick={() => window.history.back()}
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
                Submit
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default AccountForm;
