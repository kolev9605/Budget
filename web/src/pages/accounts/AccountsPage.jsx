import { NavLink } from "react-router";
import { PlusIcon, PencilIcon, BanknotesIcon, CreditCardIcon } from "@heroicons/react/24/outline";
import { useEffect, useState } from "react";
import { getAccounts } from "../../api/accounts.service.js";
import { getCurrencyLabel } from "../../utils/currencyUtils";
import LoadingOverlay from "../../components/LoadingOverlay.jsx";

const AccountsPage = () => {
  const [accounts, setAccounts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchAccounts = async () => {
      try {
        const response = await getAccounts(true);
        setAccounts(response);
      } finally {
        setIsLoading(false);
      }
    };

    fetchAccounts();
  }, []);

  return (
    <>
      {isLoading ? (
        <LoadingOverlay />
      ) : (
        <div className="min-h-screen bg-gray-900 p-6 sm:p-8 lg:p-10">
          <div className="max-w-7xl mx-auto">
            <div className="flex items-center justify-between mb-8">
              <h1 className="text-2xl font-bold text-gray-100 flex items-center gap-2">
                <BanknotesIcon className="h-6 w-6 text-blue-400" />
                My Accounts
              </h1>
              <NavLink
                to="/accounts/new"
                className="bg-blue-500 hover:bg-blue-400 text-white px-6 py-3 rounded-xl
                  flex items-center gap-2 transition-colors"
              >
                <PlusIcon className="h-5 w-5" />
                Add Account
              </NavLink>
            </div>

            {!accounts || accounts.length === 0 ? (
              <div className="bg-gray-800 p-8 rounded-2xl text-center">
                <p className="text-gray-400">No accounts found. Create your first account to get started.</p>
              </div>
            ) : (
              <>
                {/* Active Accounts */}
                <div className="grid grid-cols-1 lg:grid-cols-3 md:grid-cols-2 gap-6">
                  {accounts.filter(a => a.isActive).map((account) => (
                    <NavLink
                      to={`/accounts/edit/${account.id}`}
                      key={account.id}
                      className="bg-gray-800 p-6 rounded-2xl shadow-xl hover:shadow-2xl transition-shadow relative"
                    >
                      <div className="flex items-start justify-between mb-4">
                        <div className="flex items-center gap-4">
                          <div className="p-3 rounded-xl bg-gray-700">
                            <CreditCardIcon className="h-6 w-6 text-blue-400" />
                          </div>
                          <div>
                            <h3 className="text-lg font-semibold text-gray-100">{account.name}</h3>
                          </div>
                        </div>
                        <div className="text-gray-400 hover:text-blue-400 p-2 rounded-lg transition-colors">
                          <PencilIcon className="h-5 w-5" />
                        </div>
                      </div>
                      <div className="flex items-center justify-between mt-4">
                        <div>
                          <span className="text-sm text-gray-400">Current Balance</span>
                          <p className={`text-xl font-semibold ${account.balance < 0 ? "text-red-400" : "text-green-400"}`}>
                            {getCurrencyLabel(account.currency.abbreviation, account.balance)}
                          </p>
                        </div>
                        <span className="px-3 py-1 text-sm rounded-full bg-gray-700 text-gray-300">{account.paymentType.name}</span>
                      </div>
                    </NavLink>
                  ))}
                </div>

                {/* Hidden Accounts */}
                {accounts.some(a => !a.isActive) && (
                  <>
                    <hr className="my-10 border-gray-700" />
                    <h2 className="text-lg font-bold text-gray-400 mb-4">Hidden Accounts</h2>
                    <div className="grid grid-cols-1 lg:grid-cols-3 md:grid-cols-2 gap-6">
                      {accounts.filter(a => !a.isActive).map((account) => (
                        <NavLink
                          to={`/accounts/edit/${account.id}`}
                          key={account.id}
                          className="bg-gray-800 p-6 rounded-2xl shadow-xl hover:shadow-2xl transition-shadow relative opacity-60 grayscale"
                        >
                          <div className="flex items-start justify-between mb-4">
                            <div className="flex items-center gap-4">
                              <div className="p-3 rounded-xl bg-gray-700">
                                <CreditCardIcon className="h-6 w-6 text-blue-400" />
                              </div>
                              <div>
                                <h3 className="text-lg font-semibold text-gray-100">{account.name}</h3>
                                <span className="mt-2 inline-block bg-yellow-600 text-white text-xs font-bold px-3 py-1 rounded-full shadow">
                                  Hidden
                                </span>
                              </div>
                            </div>
                            <div className="text-gray-400 hover:text-blue-400 p-2 rounded-lg transition-colors">
                              <PencilIcon className="h-5 w-5" />
                            </div>
                          </div>
                          <div className="flex items-center justify-between mt-4">
                            <div>
                              <span className="text-sm text-gray-400">Current Balance</span>
                              <p className={`text-xl font-semibold ${account.balance < 0 ? "text-red-400" : "text-green-400"}`}>
                                {getCurrencyLabel(account.currency.abbreviation, account.balance)}
                              </p>
                            </div>
                            <span className="px-3 py-1 text-sm rounded-full bg-gray-700 text-gray-300">{account.paymentType.name}</span>
                          </div>
                        </NavLink>
                      ))}
                    </div>
                  </>
                )}
              </>
            )}
          </div>
        </div>
      )}
    </>
  );
};

export default AccountsPage;
