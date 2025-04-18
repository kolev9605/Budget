import { useNavigate } from "react-router-dom";
import RecordForm from "./RecordForm";
import { useState, useEffect } from "react";
import { useAuthContext } from "../../hooks/useAuthContext.js";
import { createAxiosAuth } from "../../api/createAxiosAuth.js";
import { getAccounts } from "../../api/accounts.service.js";
import { getCategories } from "../../api/categories.service.js";
import { getRecordTypes, createRecord } from "../../api/records.service.js";

const AddRecordPage = () => {
  const [categories, setCategories] = useState([]);
  const [accounts, setAccounts] = useState([]);
  const [recordTypes, setRecordTypes] = useState([]);
  const { user } = useAuthContext();
  const axiosAuth = createAxiosAuth(user?.token);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchData = async () => {
      const categoriesResponse = await getCategories(axiosAuth);
      const accountsResponse = await getAccounts(axiosAuth);
      const recordTypesResponse = await getRecordTypes(axiosAuth);
      setCategories(categoriesResponse);
      setAccounts(accountsResponse);
      setRecordTypes(recordTypesResponse);
    };

    fetchData();
  }, []);
  const handleSubmit = async (formData) => {
    console.log("Creating:", formData);
    await createRecord(formData, axiosAuth);
    navigate("/records"); // Redirect to transactions list
  };

  return (
    <RecordForm
      accounts={accounts}
      categories={categories}
      recordTypes={recordTypes}
      onSubmit={handleSubmit}
      record={{
        amount: "",
        recordType: "Expense",
        categoryId: "",
        note: "",
        accountId: "",
        recordDate: new Date().toISOString().split("T")[0],
      }}
    />
  );
};

export default AddRecordPage;
