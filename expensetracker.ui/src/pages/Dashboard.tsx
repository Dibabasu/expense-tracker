import React, { useState, useEffect } from 'react';
import ExpenseModal from './ExpenseModal'; // Import the modal component
import { Expense } from '../types/Expense';


const Dashboard = () => {
    const [totalExpenses, setTotalExpenses] = useState<number>(0);
    const [recentExpenses, setRecentExpenses] = useState<Expense[]>([]);
    const [filter, setFilter] = useState<string>('');
    const [sortKey, setSortKey] = useState<string>('date');
    const [sortOrder, setSortOrder] = useState<string>('asc');
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
    const [currentExpense, setCurrentExpense] = useState<Expense | null>(null);

    useEffect(() => {
        const fetchData = async () => {
            // Mock data
            const expenses: Expense[] = [
                { id: 1, description: 'Groceries', category: 'Food', amount: 50, date: '2024-06-01' },
                { id: 2, description: 'Electricity Bill', category: 'Utilities', amount: 75, date: '2024-06-02' },
                { id: 3, description: 'Clothing', category: 'Shopping', amount: 20, date: '2024-06-03' },
                { id: 4, description: 'Restaurant', category: 'Food', amount: 15, date: '2024-06-04' },
                { id: 5, description: 'Travel', category: 'Entertainment', amount: 30, date: '2024-06-05' },
            ];

            setRecentExpenses(expenses);
            setTotalExpenses(expenses.reduce((sum, expense) => sum + expense.amount, 0));
        };

        fetchData();
    }, []);

    const handleSort = (a: Expense, b: Expense) => {
        const primarySort = sortOrder === 'asc' ? 1 : -1;
        const secondarySort = sortOrder === 'asc' ? -1 : 1;

        if (sortKey === 'amount') {
            if (a.amount !== b.amount) return primarySort * (a.amount - b.amount);
        } else if (sortKey === 'description') {
            if (a.description !== b.description) return primarySort * a.description.localeCompare(b.description);
        } else if (sortKey === 'category') {
            if (a.category !== b.category) return primarySort * a.category.localeCompare(b.category);
        } else if (sortKey === 'date') {
            if (a.date !== b.date) return primarySort * (new Date(a.date).getTime() - new Date(b.date).getTime());
        }

        // Secondary sorting criteria
        if (a.description !== b.description) return secondarySort * a.description.localeCompare(b.description);
        if (a.category !== b.category) return secondarySort * a.category.localeCompare(b.category);
        if (a.amount !== b.amount) return secondarySort * (a.amount - b.amount);
        return secondarySort * (new Date(a.date).getTime() - new Date(b.date).getTime());
    };

    const filteredExpenses = recentExpenses
        .filter(expense => 
            expense.description.toLowerCase().includes(filter.toLowerCase()) || 
            expense.category.toLowerCase().includes(filter.toLowerCase())
        )
        .sort(handleSort);

    const openModal = (expense: Expense | null) => {
        setCurrentExpense(expense);
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setCurrentExpense(null);
        setIsModalOpen(false);
    };

    const handleSave = (expense: Expense) => {
        if (currentExpense) {
            setRecentExpenses(recentExpenses.map(e => e.id === expense.id ? expense : e));
        } else {
            setRecentExpenses([...recentExpenses, { ...expense, id: Date.now() }]);
        }
        setTotalExpenses(recentExpenses.reduce((sum, expense) => sum + expense.amount, 0));
        closeModal();
    };

    return (
        <div className="p-4">
            <h1 className="text-2xl font-bold mb-4">Dashboard</h1>
            <div className="mb-4">
                <h2 className="text-xl">Total Expenses: ${totalExpenses.toFixed(2)}</h2>
            </div>
            <div className="mb-4">
                <button 
                    className="bg-blue-500 text-white px-4 py-2 rounded"
                    onClick={() => openModal(null)}
                >
                    Add Expense
                </button>
            </div>
            <div>
                <h2 className="text-xl mb-2">Recent Transactions</h2>
                <div className="mb-4">
                    <input
                        type="text"
                        placeholder="Filter by description or category"
                        className="border p-2 rounded mr-4"
                        value={filter}
                        onChange={(e) => setFilter(e.target.value)}
                    />
                    <select
                        className="border p-2 rounded mr-4"
                        value={sortKey}
                        onChange={(e) => setSortKey(e.target.value)}
                    >
                        <option value="date">Date</option>
                        <option value="description">Description</option>
                        <option value="category">Category</option>
                        <option value="amount">Amount</option>
                    </select>
                    <select
                        className="border p-2 rounded"
                        value={sortOrder}
                        onChange={(e) => setSortOrder(e.target.value)}
                    >
                        <option value="asc">Ascending</option>
                        <option value="desc">Descending</option>
                    </select>
                </div>
                <div className="bg-white shadow-md rounded p-4">
                    <div className="grid grid-cols-5 gap-4 border-b pb-2 mb-2">
                        <span className="font-bold">Description</span>
                        <span className="font-bold">Category</span>
                        <span className="font-bold">Date</span>
                        <span className="font-bold">Amount</span>
                        <span className="font-bold text-center">Actions</span>
                    </div>
                    {filteredExpenses.map(expense => (
                        <div key={expense.id} className="grid grid-cols-5 gap-4 border-b py-2">
                            <span>{expense.description}</span>
                            <span>{expense.category}</span>
                            <span>{expense.date}</span>
                            <span>${expense.amount.toFixed(2)}</span>
                            <div className="text-center">
                                <button 
                                    className="text-blue-500"
                                    onClick={() => openModal(expense)}
                                >
                                    Edit
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
            {isModalOpen && (
                <ExpenseModal 
                    expense={currentExpense}
                    onSave={handleSave}
                    onClose={closeModal}
                />
            )}
        </div>
    );
};

export default Dashboard;
