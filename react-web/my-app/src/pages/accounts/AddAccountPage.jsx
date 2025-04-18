import { useNavigate } from "react-router-dom";
import AccountForm from "./AccountForm.jsx";
import { createAccount } from "../../api/accounts.service.js";
import { useAuthContext } from "../../hooks/useAuthContext.js";
import { createAxiosAuth } from "../../api/createAxiosAuth.js";

const AddAccountPage = () => {
  const navigate = useNavigate();
  const { user } = useAuthContext();
  const axiosAuth = createAxiosAuth(user?.token);

  const handleCreateAccount = async (accountData) => {
    console.log("Creating account with data:", accountData);
    const { response } = await createAccount(accountData, axiosAuth);
    console.log("submitting", response);

    navigate("/accounts");
  };

  return (
    <AccountForm
      account={{ name: "", initialBalance: 0, currencyId: "", paymentTypeId: "" }}
      onSubmit={handleCreateAccount}
      axiosAuth={axiosAuth}
    />
  );
};

export default AddAccountPage;
