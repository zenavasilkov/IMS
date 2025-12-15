import type {BoardDto} from "./dto/board_dto.ts";
import type {TicketDto} from "./dto/ticket_dto.ts";
import type {FeedbackDto} from "./dto/feedback_dto.ts";
import type {InternshipDto} from "./dto/internship_dto.ts";

interface PagedListResponse<T> {
    items: T[] | null;
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    hasPrevious: boolean;
    hasNext: boolean;
}

export interface PaginationParams {
    PageNumber: number;
    PageSize: number;
}

export type BoardDtoPagedList = PagedListResponse<BoardDto>;
export type TicketDtoPagedList = PagedListResponse<TicketDto>;
export type FeedbackDtoPagedList = PagedListResponse<FeedbackDto>;
export type InternshipDtoPagedList = PagedListResponse<InternshipDto>;
