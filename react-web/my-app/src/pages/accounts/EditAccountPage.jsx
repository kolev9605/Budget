import { useParams, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import AccountForm from "./AccountForm.jsx";
import { updateAccount, deleteAccount } from "../../api/accounts.service.js";
import { getAccountById } from "../../api/accounts.service.js";
import LoadingOverlay from "../../components/LoadingOverlay.jsx";

const EditAccountPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [account, setAccount] = useState();
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchAccountData = async () => {
      try {
        const response = await getAccountById(id);
        setAccount(response);
      } finally {
        setIsLoading(false);
      }
    };

    fetchAccountData();
  }, [id]);

  const handleUpdateAccount = async (accountData) => {
    try {
      setIsLoading(true);
      const response = await updateAccount(accountData);
      console.log("submitting", response);
    } finally {
      setIsLoading(false);
    }
    navigate("/accounts");
  };

  const handleDeleteAccount = async () => {
    if (window.confirm("Are you sure you want to delete this account?")) {
      try {
        setIsLoading(true);
        await deleteAccount(id);
      } finally {
        setIsLoading(false);
      }
      navigate("/accounts");
    }
  };

  return isLoading ? (
    <LoadingOverlay />
  ) : (
    <AccountForm account={account} onSubmit={handleUpdateAccount} onDelete={handleDeleteAccount} />
  );
};

export default EditAccountPage;
