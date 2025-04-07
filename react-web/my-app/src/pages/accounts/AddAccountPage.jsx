import { useNavigate } from "react-router-dom";
import AccountFormFields from "./AccountFormFields.jsx";
import { createAccount } from "../../api/accounts.service.js";
import { useAuthContext } from "../../hooks/useAuthContext.js";
import { createAxiosAuth } from "../../api/createAxiosAuth.js";
import { useMemo } from "react";

const AddAccountPage = () => {
  const navigate = useNavigate();
  const { user } = useAuthContext();
  const axiosAuth = useMemo(() => createAxiosAuth(user?.token), [user?.token]);

  const handleCreateAccount = async (accountData) => {
    const { response } = await createAccount(accountData, axiosAuth);
    console.log("submitting", response);

    navigate("/accounts");
  };

  return (
    <div className="min-h-screen bg-gray-900 p-6 sm:p-8 lg:p-10">
      <div className="max-w-2xl mx-auto">
        <div className="bg-gray-800 p-6 rounded-2xl shadow-xl">
          <h1 className="text-2xl font-bold text-gray-100 mb-6">Create New Account</h1>
          <AccountFormFields
            account={{ name: "", initialBalance: "", currencyId: "" }}
            onSubmit={handleCreateAccount}
            axiosAuth={axiosAuth}
          />
        </div>
      </div>
    </div>
  );
};

export default AddAccountPage;
