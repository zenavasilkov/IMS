import { createSlice, createAsyncThunk, type PayloadAction } from '@reduxjs/toolkit';
import { ticketService, feedbackService, boardService } from '../../api/services';
import type {TicketDto, CreateTicketDto, UpdateTicketDto} from '../../entities/ims/dto/ticket_dto';
import type {FeedbackDto, CreateFeedbackDto, UpdateFeedbackDto} from '../../entities/ims/dto/feedback_dto';
import { TicketStatus } from '../../entities/ims/enums';
import type {FetchFeedbacksParams} from "../../entities/ims/FetchParameters.ts";
import type { BoardDto } from "../../entities/ims/dto/board_dto.ts";

interface BoardKanbanState {
    boardId: string | null;
    boardTitle: string | null;
    tickets: TicketDto[];
    feedbacks: FeedbackDto[];
    loading: boolean;
    error: string | null;
    boardDto: BoardDto | null;
}

const initialState: BoardKanbanState = {
    boardId: null,
    boardTitle: null,
    tickets: [],
    feedbacks: [],
    loading: false,
    error: null,
    boardDto: null,
};

export const fetchBoardData = createAsyncThunk(
    'boardKanban/fetchBoardData',
    async (boardId: string, { rejectWithValue }) => {
        try {
            const board = await boardService.getBoardById(boardId);

            const ticketResult = await ticketService.getAllTickets({ BoardId: boardId, PageNumber: 1, PageSize: 1000 });
            const tickets = ticketResult.items || [];
            return { board, boardTitle: board.title, tickets };
        } catch (err: any) {
            const errorText = 'Failed to load board data.';
            console.error(err, errorText);
            return rejectWithValue(errorText);
        }
    }
);

export const updateTicketDetails = createAsyncThunk(
    'boardKanban/updateTicketDetails',
    async (
        { id, command }: { id: string, command: UpdateTicketDto },
        { rejectWithValue, dispatch }
    ) => {
        try {
            const updatedTicket = await ticketService.updateTicket(id, command);
            dispatch(fetchBoardData(updatedTicket.boardId));
            return updatedTicket;
        } catch (err: any) {
            const errorText = 'Failed to update ticket details.';
            console.error(err, errorText);
            return rejectWithValue(errorText);
        }
    }
);

export const fetchFeedbackByTicket = createAsyncThunk(
    'boardKanban/fetchFeedbackByTicket',
    async (ticketId: string, { rejectWithValue }) => {
        try {
            const params: FetchFeedbacksParams = { TicketId: ticketId, PageNumber: 1, PageSize: 100 };
            const result = await feedbackService.getAllFeedbacks(params);

            return { ticketId, feedbacks: result.items || [] };
        } catch (err: any) {
            const errorText = 'Failed to load feedback for this ticket.';
            console.error(err, errorText);
            return rejectWithValue(errorText);
        }
    }
);

export const updateTicketStatus = createAsyncThunk(
    'boardKanban/updateTicketStatus',
    async (
        { ticketId, newStatus, boardId }: { ticketId: string, newStatus: TicketStatus, boardId: string },
        { rejectWithValue, dispatch }
    ) => {
        try {
            const existingTicket = await ticketService.getTicketById(ticketId);

            const command: UpdateTicketDto = {
                title: existingTicket.title,
                description: existingTicket.description,
                deadLine: existingTicket.deadLine,
                status: newStatus,
            };

            await ticketService.updateTicket(ticketId, command);
            dispatch(updateTicketStatusLocally({ ticketId, newStatus }));
            return { ticketId, newStatus };
        } catch (err: any) {
            const errorText = 'Failed to update ticket status. Ensure all required fields (title, deadline) are present.';
            console.error(err, errorText);
            dispatch(fetchBoardData(boardId));
            return rejectWithValue(errorText);
        }
    }
);

export const createNewTicket = createAsyncThunk(
    'boardKanban/createNewTicket',
    async (command: CreateTicketDto, { rejectWithValue, dispatch }) => {
        try {
            const ticketCommand: CreateTicketDto = { ...command, status: TicketStatus.ToDo };
            const newTicket = await ticketService.createTicket(ticketCommand);

            dispatch(fetchBoardData(newTicket.boardId));
            return newTicket;
        } catch (err: any) {
            const errorText = 'Failed to create new ticket.';
            console.error(err, errorText);
            return rejectWithValue(errorText);
        }
    }
);

export const addFeedbackToTicket = createAsyncThunk(
    'boardKanban/addFeedbackToTicket',
    async (command: CreateFeedbackDto, { rejectWithValue, dispatch }) => {
        try {
            const newFeedback = await feedbackService.createFeedback(command);
            const ticket = await ticketService.getTicketById(newFeedback.ticketId);
            if (ticket.boardId) dispatch(fetchBoardData(ticket.boardId));

            return newFeedback;
        } catch (err: any) {
            const errorText ='Failed to add feedback.';
            console.error(err, errorText);
            return rejectWithValue(errorText);
        }
    }
);

export const updateFeedbackAction = createAsyncThunk(
    'boardKanban/updateFeedback',
    async ({ id, newComment }: { id: string, newComment: string }, { rejectWithValue }) => {
        try {
            const command: UpdateFeedbackDto = { comment: newComment };
            return await feedbackService.updateFeedback(id, command);
        } catch (err: any) {
            const errorText = 'Failed to update comment.';
            console.error(err, errorText);
            return rejectWithValue(errorText);
        }
    }
);

const boardKanbanSlice = createSlice({
    name: 'boardKanban',
    initialState,
    reducers: {
        setBoardId: (state, action: PayloadAction<string | null>) => {
            state.boardId = action.payload;
        },
        updateTicketStatusLocally: (state, action: PayloadAction<{ ticketId: string, newStatus: TicketStatus }>) => {
            const ticket = state.tickets.find(t => t.id === action.payload.ticketId);
            if (ticket) {
                ticket.status = action.payload.newStatus;
            }
        }
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchBoardData.pending, (state) => { state.loading = true; state.error = null; })
            .addCase(fetchBoardData.fulfilled, (state, action) => {
                state.loading = false;
                state.boardTitle = action.payload.boardTitle || null;
                state.tickets = action.payload.tickets;
                state.boardDto = action.payload.board;
            })
            .addCase(fetchBoardData.rejected, (state, action) => { state.loading = false; state.error = action.payload as string; state.tickets = []; });
    },
});

export const { setBoardId, updateTicketStatusLocally } = boardKanbanSlice.actions;
export default boardKanbanSlice.reducer;
