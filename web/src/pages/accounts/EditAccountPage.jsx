import { useParams, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import AccountForm from "./AccountForm.jsx";
import { updateAccount, deleteAccount } from "../../api/accounts.service.js";
import { getAccountById } from "../../api/accounts.service.js";
import LoadingOverlay from "../../components/LoadingOverlay.jsx";
import { toast } from "react-toastify";

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
      await updateAccount(accountData);
    } finally {
      setIsLoading(false);
    }

    toast.success("Account updated successfully!");
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

      toast.success("Account deleted successfully!");
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
