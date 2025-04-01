import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { 
  CurrencyDollarIcon,
  DocumentTextIcon,
  ArrowsRightLeftIcon,
  PlusIcon,
  PencilIcon,
  XMarkIcon
} from '@heroicons/react/24/outline';

const RecordFormPage = ({ accounts, categories, transactions }) => {
  const { id } = useParams();
  const navigate = useNavigate();
  const isEditing = !!id;

  const [formData, setFormData] = useState({
    amount: '',
    type: 'expense',
    category: '',
    note: '',
    account: '',
    destinationAccount: '',
    date: new Date().toISOString().split('T')[0],
  });

  const [errors, setErrors] = useState({});

  // Sample data - replace with your actual data
  const sampleAccounts = [
    { id: '1', name: 'Cash', type: 'cash' },
    { id: '2', name: 'Credit Card', type: 'credit' },
    { id: '3', name: 'Debit Card', type: 'debit' },
  ];

  const sampleCategories = {
    income: ['Salary', 'Freelance', 'Investment'],
    expense: ['Food', 'Transport', 'Bills', 'Entertainment'],
  };

  // Load data if editing
  useEffect(() => {
    if (isEditing) {
      // Replace with actual data fetching
      const sampleTransaction = {
        id: '1',
        amount: 245.75,
        type: 'expense',
        category: 'Food',
        note: 'Grocery shopping',
        account: '1',
        date: '2024-03-15',
      };
      
      setFormData({
        ...sampleTransaction,
        amount: Math.abs(sampleTransaction.amount).toString(),
        destinationAccount: '',
      });
    }
  }, [isEditing]);

  const validateForm = () => {
    const newErrors = {};
    
    if (!formData.amount) newErrors.amount = 'Amount is required';
    if (!formData.account) newErrors.account = 'Account is required';
    if (formData.type === 'transfer' && !formData.destinationAccount) {
      newErrors.destinationAccount = 'Destination account is required';
    }
    if (formData.type !== 'transfer' && !formData.category) {
      newErrors.category = 'Category is required';
    }
    if (formData.type === 'transfer' && formData.account === formData.destinationAccount) {
      newErrors.destinationAccount = 'Accounts must be different';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!validateForm()) return;

    const recordData = {
      ...formData,
      amount: parseFloat(formData.amount),
      date: new Date(formData.date).toISOString(),
    };

    console.log(isEditing ? 'Updating:' : 'Creating:', recordData);
    navigate('/transactions'); // Redirect to transactions list
  };

  return (
    <div className="min-h-screen bg-gray-900 p-6 sm:p-8 lg:p-10">
      <div className="max-w-2xl mx-auto">
        <div className="bg-gray-800 p-6 rounded-2xl shadow-xl">
          <div className="flex items-center justify-between mb-6">
            <h1 className="text-2xl font-bold text-gray-100 flex items-center gap-2">
              {isEditing ? (
                <>
                  <PencilIcon className="h-6 w-6 text-blue-400" />
                  Edit Transaction
                </>
              ) : (
                <>
                  <PlusIcon className="h-6 w-6 text-blue-400" />
                  Add New Transaction
                </>
              )}
            </h1>
            <button 
              onClick={() => navigate(-1)}
              className="text-gray-400 hover:text-blue-400 p-2 rounded-lg transition-colors"
            >
              <XMarkIcon className="h-6 w-6" />
            </button>
          </div>

          {Object.keys(errors).length > 0 && (
            <div className="bg-red-500/20 p-4 rounded-xl mb-6">
              <p className="text-red-400 text-sm">Please fix the following errors:</p>
              <ul className="list-disc list-inside mt-1">
                {Object.values(errors).map((error, index) => (
                  <li key={index} className="text-red-300 text-sm">{error}</li>
                ))}
              </ul>
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-6">
            {/* Transaction Type Selector */}
            <div className="grid grid-cols-3 gap-4">
              {['expense', 'income', 'transfer'].map((type) => (
                <button
                  key={type}
                  type="button"
                  onClick={() => setFormData({ ...formData, type })}
                  className={`p-3 rounded-xl flex items-center justify-center gap-2 transition-colors
                    ${formData.type === type 
                      ? 'bg-blue-500 text-white'
                      : 'bg-gray-700 hover:bg-gray-600 text-gray-300'}
                  `}
                >
                  {type === 'transfer' ? (
                    <ArrowsRightLeftIcon className="h-5 w-5" />
                  ) : (
                    <CurrencyDollarIcon className="h-5 w-5" />
                  )}
                  {type.charAt(0).toUpperCase() + type.slice(1)}
                </button>
              ))}
            </div>

            {/* Amount and Date */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label className="block text-sm font-medium text-gray-300 mb-3">
                  Amount
                </label>
                <div className="relative">
                  <input
                    type="number"
                    step="0.01"
                    value={formData.amount}
                    onChange={(e) => setFormData({ ...formData, amount: e.target.value })}
                    className="w-full bg-gray-700 border border-gray-600 rounded-xl pl-12 pr-4 py-3.5
                      text-gray-100 placeholder-gray-500 focus:outline-none focus:border-blue-400
                      focus:ring-2 focus:ring-blue-400/30 transition-all"
                    placeholder="0.00"
                  />
                  <span className="absolute left-4 top-1/2 -translate-y-1/2 text-gray-400">$</span>
                </div>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-300 mb-3">
                  Date
                </label>
                <input
                  type="date"
                  value={formData.date}
                  onChange={(e) => setFormData({ ...formData, date: e.target.value })}
                  className="w-full bg-gray-700 border border-gray-600 rounded-xl px-4 py-3.5
                    text-gray-100 focus:outline-none focus:border-blue-400 focus:ring-2 
                    focus:ring-blue-400/30"
                />
              </div>
            </div>

            {/* Accounts Section */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label className="block text-sm font-medium text-gray-300 mb-3">
                  {formData.type === 'transfer' ? 'From Account' : 'Account'}
                </label>
                <select
                  value={formData.account}
                  onChange={(e) => setFormData({ ...formData, account: e.target.value })}
                  className="w-full bg-gray-700 border border-gray-600 rounded-xl px-4 py-3.5
                    text-gray-100 focus:outline-none focus:border-blue-400 focus:ring-2 
                    focus:ring-blue-400/30 appearance-none"
                >
                  <option value="">Select Account</option>
                  {sampleAccounts.map((account) => (
                    <option key={account.id} value={account.id}>
                      {account.name}
                    </option>
                  ))}
                </select>
              </div>

              {formData.type === 'transfer' && (
                <div>
                  <label className="block text-sm font-medium text-gray-300 mb-3">
                    To Account
                  </label>
                  <select
                    value={formData.destinationAccount}
                    onChange={(e) => setFormData({ ...formData, destinationAccount: e.target.value })}
                    className="w-full bg-gray-700 border border-gray-600 rounded-xl px-4 py-3.5
                      text-gray-100 focus:outline-none focus:border-blue-400 focus:ring-2 
                      focus:ring-blue-400/30 appearance-none"
                  >
                    <option value="">Select Destination Account</option>
                    {sampleAccounts
                      .filter(acc => acc.id !== formData.account)
                      .map((account) => (
                        <option key={account.id} value={account.id}>
                          {account.name}
                        </option>
                      ))}
                  </select>
                </div>
              )}
            </div>

            {/* Category Selection */}
            {formData.type !== 'transfer' && (
              <div>
                <label className="block text-sm font-medium text-gray-300 mb-3">
                  Category
                </label>
                <select
                  value={formData.category}
                  onChange={(e) => setFormData({ ...formData, category: e.target.value })}
                  className="w-full bg-gray-700 border border-gray-600 rounded-xl px-4 py-3.5
                    text-gray-100 focus:outline-none focus:border-blue-400 focus:ring-2 
                    focus:ring-blue-400/30 appearance-none"
                >
                  <option value="">Select Category</option>
                  {sampleCategories[formData.type].map((category) => (
                    <option key={category} value={category}>
                      {category}
                    </option>
                  ))}
                </select>
              </div>
            )}

            {/* Note Field */}
            <div>
              <label className="block text-sm font-medium text-gray-300 mb-3">
                Note
              </label>
              <div className="relative">
                <textarea
                  value={formData.note}
                  onChange={(e) => setFormData({ ...formData, note: e.target.value })}
                  className="w-full bg-gray-700 border border-gray-600 rounded-xl px-4 py-3.5
                    text-gray-100 placeholder-gray-500 focus:outline-none focus:border-blue-400
                    focus:ring-2 focus:ring-blue-400/30 transition-all resize-none"
                  placeholder="Add a note..."
                  rows="3"
                />
                <DocumentTextIcon className="h-5 w-5 text-gray-500 absolute top-4 right-4" />
              </div>
            </div>

            {/* Form Actions */}
            <div className="flex flex-col-reverse sm:flex-row gap-4 mt-8">
              <button
                type="button"
                onClick={() => navigate(-1)}
                className="w-full sm:w-auto px-6 py-3 text-gray-300 hover:text-white 
                  bg-gray-700 hover:bg-gray-600 rounded-xl transition-colors"
              >
                Cancel
              </button>
              <button
                type="submit"
                className="w-full sm:w-auto px-6 py-3 bg-blue-500 hover:bg-blue-400 
                  text-white rounded-xl flex items-center justify-center gap-2 transition-colors"
              >
                {isEditing ? 'Save Changes' : 'Create Transaction'}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default RecordFormPage;