import { createSlice, createAsyncThunk, type PayloadAction } from '@reduxjs/toolkit';
import {departmentService} from "../../api/services/recruitment";
import type {GetDepartmentByIdQueryResponse} from "../../entities/recruitment/dto/department_dto.ts";

interface DepartmentState {
    departments: GetDepartmentByIdQueryResponse[];
    loading: boolean;
    error: string | null;
    page: number;
    totalPages: number;
    pageSize: number;
}

const initialState: DepartmentState = {
    departments: [],
    loading: false,
    error: null,
    page: 1,
    totalPages: 1,
    pageSize: 10,
};

export const fetchDepartments = createAsyncThunk(
    'department/fetchDepartments',
    async ({ pageNumber, pageSize }: { pageNumber: number, pageSize: number }, { rejectWithValue }) => {
        try {
            const response = await departmentService.getAllDepartments(pageNumber, pageSize);
            return {
                items: response.departments.items,
                totalCount: response.departments.totalCount || 1
            };
        } catch (err: any) {
            return rejectWithValue('Failed to load department data.');
        }
    }
);

const departmentSlice = createSlice({
    name: 'department',
    initialState,
    reducers: {
        setDepartmentPage: (state, action: PayloadAction<number>) => {
            state.page = action.payload;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchDepartments.pending, (state) => { state.loading = true; state.error = null; })
            .addCase(fetchDepartments.fulfilled, (state, action) => {
                state.loading = false;
                state.departments = action.payload.items || [];
                state.totalPages = Math.ceil(action.payload.totalCount / state.pageSize);
            })
            .addCase(fetchDepartments.rejected, (state, action) => { state.loading = false; state.error = action.payload as string; state.departments = []; });
    },
});

export const { setDepartmentPage } = departmentSlice.actions;
export default departmentSlice.reducer;
