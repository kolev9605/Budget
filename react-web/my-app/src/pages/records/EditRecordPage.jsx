import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import RecordForm from './RecordForm';
import { useAuthContext } from '../../hooks/useAuthContext.js';
import { createAxiosAuth } from '../../api/createAxiosAuth.js';
import { getAccounts } from '../../api/accounts.service.js';
import { getCategories, getCategoryTypes } from '../../api/categories.service.js';

const EditRecordPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [initialData, setInitialData] = useState(null);
  const [categories, setCategories] = useState([]);
  const [accounts, setAccounts] = useState([]);
  const [categoryTypes, setCategoryTypes] = useState([]);
  const { user } = useAuthContext();
  const axiosAuth = createAxiosAuth(user?.token);

  useEffect(() => {
    const fetchData = async () => {
      // const transactionResponse = await getTransactionById(id, axiosAuth);
      const categoriesResponse = await getCategories(axiosAuth);
      const accountsResponse = await getAccounts(axiosAuth);
      const typesResponse = await getCategoryTypes(axiosAuth);

      setInitialData({
        ...transactionResponse,
        amount: Math.abs(transactionResponse.amount).toString(),
        destinationAccount: transactionResponse.destinationAccount || '',
      });
      setCategories(categoriesResponse);
      setAccounts(accountsResponse);
      setCategoryTypes(typesResponse);
    };

    fetchData();
  }, [id]);

  const handleSubmit = (formData) => {
    console.log('Updating:', formData);
    navigate('/transactions'); // Redirect to transactions list
  };

  if (!initialData) return <div>Loading...</div>;

  return (
    <RecordForm
      accounts={accounts}
      categories={categories}
      categoryTypes={categoryTypes}
      onSubmit={handleSubmit}
      initialData={initialData}
    />
  );
};

export default EditRecordPage;
