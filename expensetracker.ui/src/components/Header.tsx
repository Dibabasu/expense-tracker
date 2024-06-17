import React from 'react';
import { Link } from 'react-router-dom';


const Header = () => {
    return (
        <header className="bg-blue-900 text-white p-4">
            <nav>
                <ul className="flex space-x-4">
                    <li><Link to="/">Dashboard</Link></li>
                    <li><Link to="/expenses">Expense List</Link></li>
                    <li><Link to="/add-expense">Add Expense</Link></li>
                </ul>
            </nav>
        </header>
    );
}

export default Header;
