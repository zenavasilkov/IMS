import {createSlice, createAsyncThunk, type PayloadAction} from '@reduxjs/toolkit';
import type { InternshipDto } from '../../entities/ims/dto/internship_dto';
import {internshipService} from "../../api/services";
import type {FetchInternshipsParams} from "../../entities/ims/FetchParameters.ts";

interface InternshipState {
    internships: InternshipDto[];
    loading: boolean;
    error: string | null;
    page: number;
    totalPages: number;
    pageSize: number;
}

const initialState: InternshipState = {
    internships: [],
    loading: false,
    error: null,
    page: 1,
    totalPages: 1,
    pageSize: 10,
};

export const fetchInternships = createAsyncThunk(
    'internship/fetchInternships',
    async (params: FetchInternshipsParams, { rejectWithValue }) => {
        try {
            const result = await internshipService.getAllInternships(params);
            return {
                items: result.items,
                totalCount: result.totalCount
            };
        } catch (err: any) {
            console.error('API Error during internships fetching:', err);
            const errorMessage = (err.response?.data?.message || err.message) || 'Failed to load internships.';
            return rejectWithValue(errorMessage);
        }
    }
);

const internshipSlice = createSlice({
    name: 'internship',
    initialState,
    reducers: {
        setInternshipPage: (state, action: PayloadAction<number>) => {
            state.page = action.payload;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchInternships.pending, (state) => { state.loading = true; state.error = null; })
            .addCase(fetchInternships.fulfilled, (state, action) => {
                state.loading = false;
                state.internships = action.payload.items || [];
                state.totalPages = Math.ceil(action.payload.totalCount / state.pageSize) || 1;
            })
            .addCase(fetchInternships.rejected, (state, action) => { state.loading = false; state.error = action.payload as string; state.internships = []; });
    },
});

export const { setInternshipPage } = internshipSlice.actions;
export default internshipSlice.reducer;
