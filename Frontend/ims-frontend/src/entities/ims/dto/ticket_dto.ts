import { TicketStatus } from "../enums";

export interface TicketDto {
    id: string;
    boardId: string;
    title?: string | null;
    description?: string | null;
    status: TicketStatus;
    deadLine: string;
}

export interface CreateTicketDto {
    boardId: string;
    title?: string | null;
    description?: string | null;
    status: TicketStatus;
    deadLine: string;
}

export interface UpdateTicketDto {
    title?: string | null;
    description?: string | null;
    status?: TicketStatus;
    deadLine?: string;
}
