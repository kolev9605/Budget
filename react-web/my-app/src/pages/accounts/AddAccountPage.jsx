import { useNavigate } from "react-router-dom";
import AccountForm from "./AccountForm.jsx";
import { createAccount } from "../../api/accounts.service.js";

const AddAccountPage = () => {
  const navigate = useNavigate();

  const handleCreateAccount = async (accountData) => {
    await createAccount(accountData);
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
