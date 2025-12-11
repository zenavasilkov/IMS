import { createSlice, createAsyncThunk, type PayloadAction } from '@reduxjs/toolkit';
import {employeeService} from "../../api/services/recruitment";
import type {GetEmployeeByIdQueryResponse} from "../../entities/recruitment/dto/employee_dto.ts";

interface EmployeeState {
    employees: GetEmployeeByIdQueryResponse[];
    loading: boolean;
    error: string | null;
    page: number;
    totalPages: number;
    pageSize: number;
}

const initialState: EmployeeState = {
    employees: [],
    loading: false,
    error: null,
    page: 1,
    totalPages: 1,
    pageSize: 10,
};

export const fetchEmployees = createAsyncThunk(
    'employee/fetchEmployees',
    async ({ pageNumber, pageSize }: { pageNumber: number, pageSize: number }, { rejectWithValue }) => {
        try {
            const response = await employeeService.getAllEmployees(pageNumber, pageSize);
            return {
                items: response.employees.items,
                totalCount: response.employees.totalCount || 1
            };
        } catch (err: any) {
            return rejectWithValue('Failed to load employee data.');
        }
    }
);

const employeeSlice = createSlice({
    name: 'employee',
    initialState,
    reducers: {
        setEmployeePage: (state, action: PayloadAction<number>) => {
            state.page = action.payload;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchEmployees.pending, (state) => { state.loading = true; state.error = null; })
            .addCase(fetchEmployees.fulfilled, (state, action) => {
                state.loading = false;
                state.employees = action.payload.items || [];
                state.totalPages = Math.ceil(action.payload.totalCount / state.pageSize);
            })
            .addCase(fetchEmployees.rejected, (state, action) => { state.loading = false; state.error = action.payload as string; state.employees = []; });
    },
});

export const { setEmployeePage } = employeeSlice.actions;
export default employeeSlice.reducer;
