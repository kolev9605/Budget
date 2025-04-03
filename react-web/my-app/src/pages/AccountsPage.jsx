import { NavLink } from "react-router";
import { PlusIcon, PencilIcon, BanknotesIcon, CreditCardIcon, WalletIcon } from "@heroicons/react/24/outline";
import Layout from "../components/Layout";

const AccountsPage = ({ accounts }) => {
  // Sample data - replace with real data
  const sampleAccounts = [
    { id: "1", name: "Cash", type: "cash", balance: 2450.75, description: "Physical cash and coins" },
    { id: "2", name: "Primary Credit Card", type: "credit", balance: -1250.0, description: "Visa Platinum **** 1234" },
    { id: "3", name: "Savings Account", type: "savings", balance: 15000.0, description: "Bank of Example - 5% APY" },
  ];

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

          {sampleAccounts.length === 0 ? (
            <div className="bg-gray-800 p-8 rounded-2xl text-center">
              <p className="text-gray-400">No accounts found. Create your first account to get started.</p>
            </div>
          ) : (
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              {sampleAccounts.map((account) => (
                <div
                  key={account.id}
                  className="bg-gray-800 p-6 rounded-2xl shadow-xl hover:shadow-2xl transition-shadow"
                >
                  <div className="flex items-start justify-between mb-4">
                    <div className="flex items-center gap-4">
                      <div className={`p-3 rounded-xl bg-gray-700`}>
                        {account.type === "credit" ? (
                          <CreditCardIcon className="h-6 w-6 text-blue-400" />
                        ) : account.type === "savings" ? (
                          <BanknotesIcon className="h-6 w-6 text-green-400" />
                        ) : (
                          <WalletIcon className="h-6 w-6 text-purple-400" />
                        )}
                      </div>
                      <div>
                        <h3 className="text-lg font-semibold text-gray-100">{account.name}</h3>
                        <p className="text-sm text-gray-400">{account.description}</p>
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
                    <span className="px-3 py-1 text-sm rounded-full bg-gray-700 text-gray-300">
                      {account.type.charAt(0).toUpperCase() + account.type.slice(1)}
                    </span>
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
