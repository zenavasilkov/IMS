import {type InternshipStatus, Role, TicketStatus, UserSortingParameter} from "./enums.ts";
import type {PaginationParams} from "./Pagination.ts";

export interface FetchBoardsParams extends PaginationParams {
    Title?: string;
    Description?: string;
    CreatedById?: string;
    CreatedToId?: string;
}

export interface FetchTicketsParams extends PaginationParams {
    BoardId?: string;
    Title?: string;
    Description?: string;
    Status?: TicketStatus;
    DeadLine?: string;
}

export interface FetchFeedbacksParams extends PaginationParams {
    TicketId?: string;
    SentById?: string;
    SentToId?: string;
    Comment?: string;
}

export interface FetchInternshipsParams extends PaginationParams {
    InternId?: string;
    MentorId?: string;
    HrManagerId?: string;
    StartedAfter?: string;
    StartedBefore?: string;
    Status?: InternshipStatus;
}

export interface FetchUsersParams extends PaginationParams {
    FirstName?: string;
    LastName?: string;
    Role?: Role;
    Sorter?: UserSortingParameter;
    Email?: string;
}
