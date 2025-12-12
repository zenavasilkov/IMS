import { createSlice, createAsyncThunk, type PayloadAction } from '@reduxjs/toolkit';
import type {AddFeedbackCommand, GetInterviewByIdQueryResponse} from "../../entities/recruitment/dto/interview_dto.ts";
import {fetchCandidateByEmail} from "./recruitmentSlice.ts";
import {interviewService} from "../../api/services/recruitment";

export interface FetchInterviewsParams {
    pageNumber: number;
    pageSize: number;
    candidateId?: string;
}

interface InterviewState {
    interviews: GetInterviewByIdQueryResponse[];
    loading: boolean;
    error: string | null;
    page: number;
    totalPages: number;
    pageSize: number;

    filterCandidateEmail: string;
    filterCandidateId: string | undefined;
}

const initialState: InterviewState = {
    interviews: [],
    loading: false,
    error: null,
    page: 1,
    totalPages: 1,
    pageSize: 10,

    filterCandidateEmail: '',
    filterCandidateId: undefined,
};

export const passInterview = createAsyncThunk<string, string, { state: { interview: InterviewState } }>(
    'interview/passInterview',
    async (interviewId, { dispatch, getState, rejectWithValue }) => {
        try {
            const command: AddFeedbackCommand = {
                id: interviewId,
                isPassed: true,
                feedback: 'Interview passed successfully.',
            };

            await interviewService.addFeedback(command);

            const state = getState() as { interview: InterviewState };
            const params = { pageNumber: state.interview.page, pageSize: state.interview.pageSize };
            dispatch(fetchInterviews(params));

            return interviewId;
        } catch (err: any) {
            let errorMessage = 'Failed to mark interview as passed.';

            if (err.response) {
                if (typeof err.response.data === 'string' && err.response.data.includes('Cannot add a feedback')) {
                    errorMessage = err.response.data;
                }
                else if (err.response.data?.message) {
                    errorMessage = err.response.data.message;
                }
                else if (err.response.data) {
                    errorMessage = err.response.data;
                }
            }

            return rejectWithValue(errorMessage);
        }
    }
);

export const fetchInterviews = createAsyncThunk(
    'interview/fetchInterviews',
    async (params: FetchInterviewsParams, { getState, rejectWithValue }) => {
        try {
            const state = getState() as { interview: InterviewState };
            const candidateId = state.interview.filterCandidateId;

            const response = candidateId
                ? await interviewService.getInterviewsByCandidate(candidateId, params.pageNumber, params.pageSize)
                : await interviewService.getAllInterviews(params.pageNumber, params.pageSize);

            return {
                items: response.interviews.items,
                totalCount: response.interviews.totalCount
            };
        } catch (err: any) {
            return rejectWithValue('Failed to load interviews.');
        }
    }
);

const interviewSlice = createSlice({
    name: 'interview',
    initialState,
    reducers: {
        setInterviewPage: (state, action: PayloadAction<number>) => {
            state.page = action.payload;
        },
        setFilterCandidateEmail: (state, action: PayloadAction<string>) => {
            state.filterCandidateEmail = action.payload;
        },
        setFilterCandidateId: (state, action: PayloadAction<string | undefined>) => {
            state.filterCandidateId = action.payload;
            state.page = 1;
        },
        resetInterviewFilters: (state) => {
            state.filterCandidateEmail = initialState.filterCandidateEmail;
            state.filterCandidateId = initialState.filterCandidateId;
            state.page = 1;
        }
    },
    extraReducers: (builder) => {
        builder.addCase(fetchCandidateByEmail.fulfilled, (state, action) => {
            state.filterCandidateId = action.payload?.id ?? '';
        });

        builder
            .addCase(fetchInterviews.pending, (state) => { state.loading = true; state.error = null; })
            .addCase(fetchInterviews.fulfilled, (state, action) => {
                state.loading = false;
                state.interviews = action.payload.items || [];
                state.totalPages = Math.ceil(action.payload.totalCount / state.pageSize);
            })
            .addCase(fetchInterviews.rejected, (state, action) =>
                { state.loading = false; state.error = action.payload as string; state.interviews = []; });
    },
});

export const { setInterviewPage, setFilterCandidateEmail, setFilterCandidateId, resetInterviewFilters } = interviewSlice.actions;
export default interviewSlice.reducer;
