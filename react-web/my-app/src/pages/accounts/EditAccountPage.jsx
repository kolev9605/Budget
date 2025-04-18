import { useParams, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import AccountForm from "./AccountForm.jsx";
import { updateAccount, deleteAccount } from "../../api/accounts.service.js";
import { getAccountById } from "../../api/accounts.service.js";
import { useAuthContext } from "../../hooks/useAuthContext.js";
import { createAxiosAuth } from "../../api/createAxiosAuth.js";

const EditAccountPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { user } = useAuthContext();
  const axiosAuth = createAxiosAuth(user?.token);
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
  }, [id]);

  const handleUpdateAccount = async (accountData) => {
    const response = await updateAccount(accountData, axiosAuth);
    console.log("submitting", response);
    navigate("/accounts");
  };

  const handleDeleteAccount = async () => {
    if (window.confirm("Are you sure you want to delete this account?")) {
      await deleteAccount(id, axiosAuth);
      navigate("/accounts");
    }
  };

  return isLoading ? ( // Conditionally render loading or form
    <p className="text-gray-400">Loading...</p>
  ) : (
    <>
      <AccountForm account={account} onSubmit={handleUpdateAccount} axiosAuth={axiosAuth} />
      <button
        onClick={handleDeleteAccount}
        className="mt-4 w-full bg-red-500 text-white py-2 px-4 rounded-md hover:bg-red-600 focus:outline-none focus:ring-2 focus:ring-red-400 focus:ring-offset-2"
      >
        Delete Account
      </button>
    </>
  );
};

export default EditAccountPage;
