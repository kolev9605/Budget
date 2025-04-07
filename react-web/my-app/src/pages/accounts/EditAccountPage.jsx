import { useParams, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import AccountFormFields from "./AccountFormFields.jsx";
import { updateAccount } from "../../api/accounts.service.js";
import { getAccountById } from "../../api/accounts.service.js";
import { useAuthContext } from "../../hooks/useAuthContext.js";
import { createAxiosAuth } from "../../api/createAxiosAuth.js";
import { useMemo } from "react";

const EditAccountPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { user } = useAuthContext();
  const axiosAuth = useMemo(() => createAxiosAuth(user?.token), [user?.token]);
  const [account, setAccount] = useState();
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchAccountData = async () => {
      const response = await getAccountById(id, axiosAuth);
      setAccount(response);
      setIsLoading(false); // Set loading to false after data is fetched
      console.log(response);
    };

    fetchAccountData();
  }, [axiosAuth, id]);

  const handleUpdateAccount = async (accountData) => {
    const response = await updateAccount(accountData, axiosAuth);
    console.log("submitting", response);
    navigate("/accounts");
  };

  return (
    <div className="min-h-screen bg-gray-900 p-6 sm:p-8 lg:p-10">
      <div className="max-w-2xl mx-auto">
        <div className="bg-gray-800 p-6 rounded-2xl shadow-xl">
          <h1 className="text-2xl font-bold text-gray-100 mb-6">Edit Account</h1>
          {isLoading ? ( // Conditionally render loading or form
            <p className="text-gray-400">Loading...</p>
          ) : (
            <AccountFormFields account={account} onSubmit={handleUpdateAccount} axiosAuth={axiosAuth} />
          )}
        </div>
      </div>
    </div>
  );
};

export default EditAccountPage;
