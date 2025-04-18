import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import RecordForm from "./RecordForm";
import { useAuthContext } from "../../hooks/useAuthContext.js";
import { createAxiosAuth } from "../../api/createAxiosAuth.js";
import { getAccounts } from "../../api/accounts.service.js";
import { getCategories } from "../../api/categories.service.js";
import { getRecordById, getRecordTypes, updateRecord } from "../../api/records.service.js";

const EditRecordPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [record, setRecord] = useState(null);
  const [categories, setCategories] = useState([]);
  const [accounts, setAccounts] = useState([]);
  const [recordTypes, setRecordTypes] = useState([]);
  const { user } = useAuthContext();
  const axiosAuth = createAxiosAuth(user?.token);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      const categoriesResponse = await getCategories(axiosAuth);
      const accountsResponse = await getAccounts(axiosAuth);
      const recordResponse = await getRecordById(id, axiosAuth);
      const recordTypesResponse = await getRecordTypes(axiosAuth);

      setRecord({
        id: recordResponse.id || "",
        amount: Math.abs(recordResponse.amount),
        fromAccountId: recordResponse.fromAccount?.id || null,
        categoryId: recordResponse.category.id || "",
        accountId: recordResponse.account.id || "",
        // todo: not optimal conversion
        recordDate: new Date(recordResponse.recordDate).toISOString().split("T")[0],
        note: recordResponse.note || "",
        recordType: recordResponse.recordType,
      });

      setCategories(categoriesResponse);
      setAccounts(accountsResponse);
      setRecordTypes(recordTypesResponse);
      setIsLoading(false); // Set loading to false after data is fetched
    };

    fetchData();
  }, [id]);

  const handleSubmit = async (formData) => {
    console.log("Updating:", formData);
    await updateRecord(formData, axiosAuth);
    navigate("/records"); // Redirect to transactions list
  };

  if (!record) return <div>Loading...</div>;

  return isLoading ? ( // Conditionally render loading or form
    <p className="text-gray-400">Loading...</p>
  ) : (
    <RecordForm
      accounts={accounts}
      categories={categories}
      recordTypes={recordTypes}
      onSubmit={handleSubmit}
      record={record}
    />
  );
};

export default EditRecordPage;
