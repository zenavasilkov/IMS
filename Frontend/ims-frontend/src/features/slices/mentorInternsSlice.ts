import { createSlice, createAsyncThunk, type PayloadAction } from '@reduxjs/toolkit';
import {boardService, internshipService, userService} from '../../api/services';
import type { InternshipDto } from '../../entities/ims/dto/internship_dto';
import type { UserDto } from '../../entities/ims/dto/user_dto';
import type {FetchBoardsParams, FetchInternshipsParams} from '../../entities/ims/FetchParameters';
import type {BoardDto} from "../../entities/ims/dto/board_dto.ts";

export interface InternWithInternship extends UserDto {
    assignedInternship: InternshipDto;
    hasBoard: boolean;
}

interface MentorInternsState {
    interns: InternWithInternship[];
    loading: boolean;
    error: string | null;
    page: number;
    totalPages: number;
    pageSize: number;
    mentorId: string | null;
}

const initialState: MentorInternsState = {
    interns: [],
    loading: false,
    error: null,
    page: 1,
    totalPages: 1,
    pageSize: 10,
    mentorId: null,
};

export const fetchBoardByInternId = async (internId: string): Promise<BoardDto | undefined> => {
    const params: FetchBoardsParams = {
        CreatedToId: internId,
        PageNumber: 1,
        PageSize: 1,
    };
    try {
        const result = await boardService.getAllBoards(params);
        return result.items?.[0];
    } catch (e) {
        console.error("Failed to fetch board by Intern ID:", e);
        return undefined;
    }
};

export const fetchMentorInterns = createAsyncThunk(
    'mentorInterns/fetchInterns',
    async (mentorId: string, { rejectWithValue }) => {
        try {
            const internshipParams: FetchInternshipsParams = { MentorId: mentorId, PageNumber: 1, PageSize: 1000 };
            const internshipResult = await internshipService.getAllInternships(internshipParams);
            const internships = internshipResult.items || [];
            const internIds = Array.from(new Set(internships.map(i => i.internId)));
            const internPromises = internIds.map(id => userService.getUserById(id));
            const internDtos = (await Promise.all(internPromises)).filter(u => u !== undefined) as UserDto[];
            const internMap = new Map<string, UserDto>(internDtos.map(u => [u.id, u]));
            const internshipMap = new Map<string, InternshipDto>(internships.map(i => [i.internId, i]));

            const internPromisesWithBoards = internIds.map(async (id) => {
                const user = internMap.get(id);
                const internship = internshipMap.get(id);

                if (user && internship) {
                    const board = await fetchBoardByInternId(id);

                    return {
                        ...user,
                        assignedInternship: internship,
                        hasBoard: board !== undefined,
                    } as InternWithInternship;
                }
                return null;
            });

            const results = await Promise.all(internPromisesWithBoards);

            const finalInterns: InternWithInternship[] = results.filter(
                (i): i is InternWithInternship => i !== null
            );

            return { items: finalInterns };
        } catch (err: any) {
            return rejectWithValue('Failed to load mentor interns.');
        }
    }
);

const mentorInternsSlice = createSlice({
    name: 'mentorInterns',
    initialState,
    reducers: {
        setMentorId: (state, action: PayloadAction<string | null>) => { state.mentorId = action.payload; },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchMentorInterns.pending, (state) => { state.loading = true; state.error = null; })
            .addCase(fetchMentorInterns.fulfilled, (state, action) => {
                state.loading = false;
                state.interns = action.payload.items;
                state.totalPages = Math.ceil(state.interns.length / state.pageSize);
                state.page = 1;
            })
            .addCase(fetchMentorInterns.rejected, (state, action) => { state.loading = false; state.error = action.payload as string; state.interns = []; });
    },
});

export const { setMentorId } = mentorInternsSlice.actions;
export default mentorInternsSlice.reducer;
