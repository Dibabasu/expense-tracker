import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import axiosInstance from '../../api/axiosInstance';
import { Expense } from '../../types/Expense';

interface ExpensesState {
  totalExpenses: number;
  recentExpenses: Expense[];
  loading: boolean;
  error: string | null;
}

const initialState: ExpensesState = {
  totalExpenses: 0,
  recentExpenses: [],
  loading: false,
  error: null,
};

export const fetchExpenses = createAsyncThunk(
  'expenses/fetchExpenses',
  async ({ pageNumber, pageSize }: { pageNumber: number; pageSize: number }) => {
    const response = await axiosInstance.get(`/Expenses?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    return response.data.items; // Updated from response.data
  }
);


const expensesSlice = createSlice({
  name: 'expenses',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchExpenses.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchExpenses.fulfilled, (state, action) => {
        state.loading = false;
        state.recentExpenses = action.payload; // Updated from action.payload.expenses
        state.totalExpenses = action.payload
          ? action.payload.reduce((sum: number, expense: Expense) => sum + expense.amount, 0)
          : 0;
      })
      .addCase(fetchExpenses.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message || 'Failed to fetch expenses';
      });
  },
});

export default expensesSlice.reducer;
