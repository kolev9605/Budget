import { useState } from 'react';
import { Link } from 'react-router-dom';
import {
  PlusIcon,
  ArrowsUpDownIcon,
  FunnelIcon,
  MagnifyingGlassIcon,
  PencilIcon,
  TrashIcon,
  ArrowRightCircleIcon
} from '@heroicons/react/24/outline';

const RecordsPage = ({ records }) => {
  const [searchTerm, setSearchTerm] = useState('');
  const [filterType, setFilterType] = useState('all');
  const [sortBy, setSortBy] = useState('date');

  // Sample data - replace with real data
  const sampleRecords = [
    {
      id: 1,
      date: '2024-03-15',
      amount: -245.75,
      category: 'Food',
      type: 'expense',
      account: 'Cash',
      note: 'Grocery shopping',
      destinationAccount: ''
    },
    {
      id: 2,
      date: '2024-03-14',
      amount: 5000.00,
      category: 'Salary',
      type: 'income',
      account: 'Bank Account',
      note: 'Monthly salary',
      destinationAccount: ''
    },
    {
      id: 3,
      date: '2024-03-13',
      amount: -1000.00,
      type: 'transfer',
      account: 'Savings',
      note: 'Monthly savings',
      destinationAccount: 'Investment Account'
    }
  ];

  const filteredRecords = sampleRecords
    .filter(record => {
      const matchesSearch = record.note.toLowerCase().includes(searchTerm.toLowerCase()) ||
                          record.category.toLowerCase().includes(searchTerm.toLowerCase());
      const matchesType = filterType === 'all' || record.type === filterType;
      return matchesSearch && matchesType;
    })
    .sort((a, b) => {
      if(sortBy === 'date') return new Date(b.date) - new Date(a.date);
      if(sortBy === 'amount') return Math.abs(b.amount) - Math.abs(a.amount);
      return 0;
    });

  return (
    <div className="min-h-screen bg-gray-900 p-6 sm:p-8 lg:p-10">
      <div className="max-w-7xl mx-auto">
        {/* Header */}
        <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between mb-8 gap-4">
          <div>
            <h1 className="text-2xl font-bold text-gray-100">Transaction History</h1>
            <p className="text-gray-400 text-sm mt-1">
              {filteredRecords.length} transactions found
            </p>
          </div>
          <div className="flex gap-4">
            <Link
              to="/records/new"
              className="bg-blue-500 hover:bg-blue-400 text-white px-6 py-3 rounded-xl
                flex items-center gap-2 transition-colors"
            >
              <PlusIcon className="h-5 w-5" />
              New Record
            </Link>
          </div>
        </div>

        {/* Filters */}
        <div className="bg-gray-800 rounded-2xl p-6 mb-8 shadow-xl">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div className="relative">
              <input
                type="text"
                placeholder="Search transactions..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="w-full bg-gray-700 border border-gray-600 rounded-xl pl-12 pr-4 py-3
                  text-gray-100 placeholder-gray-500 focus:outline-none focus:border-blue-400
                  focus:ring-2 focus:ring-blue-400/30"
              />
              <MagnifyingGlassIcon className="h-5 w-5 text-gray-400 absolute left-4 top-1/2 -translate-y-1/2" />
            </div>

            <select
              value={filterType}
              onChange={(e) => setFilterType(e.target.value)}
              className="bg-gray-700 border border-gray-600 rounded-xl px-4 py-3
                text-gray-100 focus:outline-none focus:border-blue-400 focus:ring-2 
                focus:ring-blue-400/30"
            >
              <option value="all">All Types</option>
              <option value="income">Income</option>
              <option value="expense">Expense</option>
              <option value="transfer">Transfer</option>
            </select>

            <select
              value={sortBy}
              onChange={(e) => setSortBy(e.target.value)}
              className="bg-gray-700 border border-gray-600 rounded-xl px-4 py-3
                text-gray-100 focus:outline-none focus:border-blue-400 focus:ring-2 
                focus:ring-blue-400/30"
            >
              <option value="date">Sort by Date</option>
              <option value="amount">Sort by Amount</option>
            </select>
          </div>
        </div>

        {/* Records List */}
        <div className="space-y-4">
          {filteredRecords.map(record => (
            <div 
              key={record.id}
              className="bg-gray-800 p-6 rounded-2xl shadow-xl hover:shadow-2xl transition-shadow"
            >
              <div className="flex flex-col md:flex-row items-start md:items-center justify-between gap-4">
                {/* Left Section */}
                <div className="flex-1">
                  <div className="flex items-center gap-4 mb-2">
                    <span className={`text-sm px-3 py-1 rounded-full ${
                      record.type === 'income' ? 'bg-green-500/20 text-green-400' :
                      record.type === 'expense' ? 'bg-red-500/20 text-red-400' :
                      'bg-blue-500/20 text-blue-400'
                    }`}>
                      {record.type.charAt(0).toUpperCase() + record.type.slice(1)}
                    </span>
                    <span className="text-gray-400 text-sm">
                      {new Date(record.date).toLocaleDateString()}
                    </span>
                  </div>
                  <h3 className="text-gray-100 font-medium">{record.note}</h3>
                  <p className="text-gray-400 text-sm mt-1">
                    {record.category} • {record.account}
                    {record.destinationAccount && (
                      <span className="flex items-center gap-1">
                        <ArrowRightCircleIcon className="h-4 w-4 mx-2" />
                        {record.destinationAccount}
                      </span>
                    )}
                  </p>
                </div>

                {/* Right Section */}
                <div className="flex items-center gap-6">
                  <p className={`text-xl font-semibold ${
                    record.type === 'income' ? 'text-green-400' :
                    record.type === 'expense' ? 'text-red-400' :
                    'text-blue-400'
                  }`}>
                    {record.type !== 'transfer' && (
                      record.type === 'income' ? '+' : '-'
                    )}
                    ${Math.abs(record.amount).toLocaleString(undefined, {
                      minimumFractionDigits: 2,
                      maximumFractionDigits: 2
                    })}
                  </p>
                  <div className="flex items-center gap-2">
                    <Link
                      to={`/records/edit/${record.id}`}
                      className="text-gray-400 hover:text-blue-400 p-2 rounded-lg transition-colors"
                    >
                      <PencilIcon className="h-5 w-5" />
                    </Link>
                    <button className="text-gray-400 hover:text-red-400 p-2 rounded-lg transition-colors">
                      <TrashIcon className="h-5 w-5" />
                    </button>
                  </div>
                </div>
              </div>
            </div>
          ))}

          {filteredRecords.length === 0 && (
            <div className="bg-gray-800 p-8 rounded-2xl text-center">
              <p className="text-gray-400">No transactions found. Create your first record!</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default RecordsPage;