import { NavLink } from "react-router";
import { PlusIcon, PencilIcon, BanknotesIcon, CreditCardIcon, WalletIcon } from "@heroicons/react/24/outline";
import { useEffect, useState, useMemo } from "react";
import { createAxiosAuth } from "../../api/createAxiosAuth.js";
import { useAuthContext } from "../../hooks/useAuthContext.js";
import { getAccounts } from "../../api/accounts.service.js";

const AccountsPage = () => {
  const [accounts, setAccounts] = useState([]);
  const { user } = useAuthContext();
  const axiosAuth = useMemo(() => createAxiosAuth(user?.token), [user?.token]);

  useEffect(() => {
    const fetchAccounts = async () => {
      const response = await getAccounts(axiosAuth);
      console.log(response);
      
      setAccounts(response);
    };

    fetchAccounts();
  }, [axiosAuth]);

  return (
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
          <div className="grid grid-cols-1 lg:grid-cols-3 md:grid-cols-2 gap-6">
            {accounts.map((account) => (
              <div
                key={account.id}
                className="bg-gray-800 p-6 rounded-2xl shadow-xl hover:shadow-2xl transition-shadow"
              >
                <div className="flex items-start justify-between mb-4">
                  <div className="flex items-center gap-4">
                    <div className={`p-3 rounded-xl bg-gray-700`}>
                      <CreditCardIcon className="h-6 w-6 text-blue-400" />
                      {/* {account.type === "credit" ? (
                          <CreditCardIcon className="h-6 w-6 text-blue-400" />
                        ) : account.type === "savings" ? (
                          <BanknotesIcon className="h-6 w-6 text-green-400" />
                        ) : (
                          <WalletIcon className="h-6 w-6 text-purple-400" />
                        )} */}
                    </div>
                    <div>
                      <h3 className="text-lg font-semibold text-gray-100">{account.name}</h3>
                      {/* <p className="text-sm text-gray-400">{account.description}</p> */}
                    </div>
                  </div>
                  <NavLink
                    to={`/accounts/edit/${account.id}`}
                    className="text-gray-400 hover:text-blue-400 p-2 rounded-lg transition-colors"
                  >
                    <PencilIcon className="h-5 w-5" />
                  </NavLink>
                </div>

                <div className="flex items-center justify-between mt-4">
                  <div>
                    <span className="text-sm text-gray-400">Current Balance</span>
                    <p className={`text-xl font-semibold ${account.balance < 0 ? "text-red-400" : "text-green-400"}`}>
                      $
                      {Math.abs(account.balance).toLocaleString(undefined, {
                        minimumFractionDigits: 2,
                        maximumFractionDigits: 2,
                      })}
                    </p>
                  </div>
                  <span className="px-3 py-1 text-sm rounded-full bg-gray-700 text-gray-300">Credit</span>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default AccountsPage;
