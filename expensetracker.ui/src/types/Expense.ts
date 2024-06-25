import { Category } from "./Category";

export interface Expense {
    id?: string; // Updated from number to string based on your API response
    description: string;
    amount: number;
    date: string;
    category: Category; // Updated from string to number based on your API response
    links?: Array<{ href: string; rel: string; method: string }>; // New property based on your API response
  }