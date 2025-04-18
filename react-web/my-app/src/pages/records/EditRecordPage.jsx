import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import RecordForm from "./RecordForm";
import axiosInstance from "../../api/createAxiosAuth.js";

const EditRecordPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [record, setRecord] = useState(null);
  const [categories, setCategories] = useState([]);
  const [accounts, setAccounts] = useState([]);
  const [recordTypes, setRecordTypes] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      const categoriesResponse = await axiosInstance.get("/categories");
      const accountsResponse = await axiosInstance.get("/accounts");
      const recordResponse = await axiosInstance.get(`/records/${id}`);
      const recordTypesResponse = await axiosInstance.get("/records/types");

      setRecord({
        id: recordResponse.data.id || "",
        amount: Math.abs(recordResponse.data.amount),
        fromAccountId: recordResponse.data.fromAccount?.id || null,
        categoryId: recordResponse.data.category.id || "",
        accountId: recordResponse.data.account.id || "",
        recordDate: new Date(recordResponse.data.recordDate).toISOString().split("T")[0],
        note: recordResponse.data.note || "",
        recordType: recordResponse.data.recordType,
      });

      setCategories(categoriesResponse.data);
      setAccounts(accountsResponse.data);
      setRecordTypes(recordTypesResponse.data);
      setIsLoading(false);
    };

    fetchData();
  }, [id]);

  const handleSubmit = async (formData) => {
    await axiosInstance.put(`/records/${formData.id}`, formData);
    navigate("/records");
  };

  if (!record) return <div>Loading...</div>;

  return isLoading ? (
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
