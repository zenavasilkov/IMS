import { createSlice, createAsyncThunk, type PayloadAction } from '@reduxjs/toolkit';
import { candidateService } from '../../api/services/recruitment';
import type {
    FindCandidateByIdQueryResponse,
    UpdateCvLinkCommand
} from '../../entities/recruitment/dto/candidate_dto.ts';

export interface FetchCandidatesParams {
    pageNumber: number;
    pageSize: number;
}

interface RecruitmentState {
    candidates: FindCandidateByIdQueryResponse[];
    loading: boolean;
    error: string | null;
    page: number;
    totalPages: number;
    pageSize: number;
    acceptingCandidates: string[];
    filterEmail: string;
}

const initialState: RecruitmentState = {
    candidates: [],
    loading: false,
    error: null,
    page: 1,
    totalPages: 1,
    pageSize: 10,
    acceptingCandidates: [],
    filterEmail: ''
};

export const fetchCandidateByEmail = createAsyncThunk(
    'interview/fetchCandidateIdByEmail',
    async (email: string, { rejectWithValue }) => {
        try {
            let result: FindCandidateByIdQueryResponse;
            result = await candidateService.getCandidateByEmail(email) as FindCandidateByIdQueryResponse;
            return result;
        } catch (err: any) {
            if (err.response?.status === 404) return undefined;
            return rejectWithValue('Failed to search candidate by email.');
        }
    }
);

export const fetchCandidates = createAsyncThunk(
    'recruitment/fetchCandidates',
    async (params: FetchCandidatesParams, { rejectWithValue }) => {
        try {
            const result = await candidateService.getAllCandidates(params.pageNumber, params.pageSize);
            return {
                items: result.candidates.items,
                totalCount: result.candidates.totalCount || 1
            };
        } catch (err: any) {
            const errorMessage = err.response?.data?.message || err.message || "An unknown error occurred.";
            return rejectWithValue(`Failed to load candidates: ${errorMessage}`);
        }
    }
);

export const acceptCandidate = createAsyncThunk<string, string,  { state: { recruitment: RecruitmentState } } >(
    'recruitment/acceptCandidate',
    async (candidateId, { rejectWithValue }) => {
        try {
            await candidateService.acceptToInternship(candidateId);
            return candidateId;
        } catch (err: any) {
            const errorMessage = err.response?.data?.message || 'Failed to accept candidate.';
            return rejectWithValue(errorMessage);
        }
    }
);

export const updateCandidateCv = createAsyncThunk<string,UpdateCvLinkCommand, { state: { recruitment: RecruitmentState } }>(
    'recruitment/updateCandidateCv',
    async (command, { rejectWithValue }) => {
        try {
            await candidateService.updateCvLink(command);
            return command.id;
        } catch (err: any) {
            console.error('API Error during CV link update:', err);
            const errorMessage = (err.response?.data?.message || err.message) || 'Failed to update CV link.';
            return rejectWithValue(errorMessage);
        }
    }
);

const recruitmentSlice = createSlice({
    name: 'recruitment',
    initialState,
    reducers: {
        setCandidatePage: (state, action: PayloadAction<number>) => {
            state.page = action.payload;
        },
        setFilterEmail: (state, action: PayloadAction<string>) => {
            state.filterEmail = action.payload;
        },
        resetRecruitmentFilters: (state) => {
            state.filterEmail = initialState.filterEmail;
            state.page = 1;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchCandidateByEmail.fulfilled, (state, action) => {
                const candidate = action.payload;

                state.candidates = candidate ? [candidate] : [];

                state.totalPages = 1;
                state.page = 1;
                state.loading = false;
                state.error = null;
            })
            .addCase(fetchCandidateByEmail.rejected, (state, action) => {
                state.candidates = [];
                state.totalPages = 1;
                state.page = 1;
                state.loading = false;
                state.error = action.payload as string;
            })
            .addCase(fetchCandidates.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(fetchCandidates.fulfilled, (state, action) => {
                state.loading = false;
                state.candidates = action.payload.items || [];
                state.totalPages = Math.ceil(action.payload.totalCount / state.pageSize);
            })
            .addCase(fetchCandidates.rejected, (state, action) => {
                state.loading = false;
                state.error = action.payload as string;
                state.candidates = [];
            })
            .addCase(updateCandidateCv.rejected, (state, action) => {
                state.error = action.payload as string;
            })
            .addCase(acceptCandidate.pending, (state, action) => {
                state.acceptingCandidates.push(action.meta.arg);
                state.error = null;
            })
            .addCase(acceptCandidate.fulfilled, (state, action) => {
                state.acceptingCandidates = state.acceptingCandidates.filter(id => id !== action.payload);
                state.error = null;
            })
            .addCase(acceptCandidate.rejected, (state, action) => {
                const candidateId = action.meta.arg;
                state.acceptingCandidates = state.acceptingCandidates.filter(id => id !== candidateId);
                state.error = action.payload as string;
            })
    },
});

export const { setCandidatePage, setFilterEmail, resetRecruitmentFilters } = recruitmentSlice.actions;
export default recruitmentSlice.reducer;
