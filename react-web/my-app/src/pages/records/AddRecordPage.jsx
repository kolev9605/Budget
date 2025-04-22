import { useNavigate } from "react-router-dom";
import RecordForm from "./RecordForm";
import { useState, useEffect } from "react";
import { getAccounts } from "../../api/accounts.service.js";
import { getCategories } from "../../api/categories.service.js";
import { getRecordTypes, createRecord } from "../../api/records.service.js";
import axiosInstance from "../../api/createAxiosAuth.js";
import LoadingOverlay from "../../components/LoadingOverlay.jsx";

const AddRecordPage = () => {
  const [categories, setCategories] = useState([]);
  const [accounts, setAccounts] = useState([]);
  const [recordTypes, setRecordTypes] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const categoriesResponse = await getCategories(axiosInstance);
        const accountsResponse = await getAccounts(axiosInstance);
        const recordTypesResponse = await getRecordTypes(axiosInstance);
        setCategories(categoriesResponse);
        setAccounts(accountsResponse);
        setRecordTypes(recordTypesResponse);
      } finally {
        setIsLoading(false);
      }
    };

    fetchData();
  }, []);

  const handleSubmit = async (formData) => {
    console.log("Creating:", formData);
    await createRecord(formData, axiosInstance);
    navigate("/records");
  };

  return isLoading ? (
    <LoadingOverlay />
  ) : (
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
