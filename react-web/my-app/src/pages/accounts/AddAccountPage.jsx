import { useNavigate } from "react-router-dom";
import AccountForm from "./AccountForm.jsx";
import { createAccount } from "../../api/accounts.service.js";
import { toast } from "react-toastify";

const AddAccountPage = () => {
  const navigate = useNavigate();

  const handleCreateAccount = async (accountData) => {
    await createAccount(accountData);
    toast.success("Account created successfully!");
    navigate("/accounts");
  };

  return (
    <AccountForm
      account={{ name: "", initialBalance: 0, currencyId: "", paymentTypeId: "" }}
      onSubmit={handleCreateAccount}
    />
  );
};

export default AddAccountPage;
