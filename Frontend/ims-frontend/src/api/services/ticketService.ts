import { ImsApi } from '../axios';
import type { TicketDto, CreateTicketDto, UpdateTicketDto } from '../../entities/ims/dto/ticket_dto';

export const ticketService = {
    getAllTickets: async (): Promise<TicketDto[]> => {
        const response = await ImsApi.get<TicketDto[]>('/tickets');
        return response.data;
    },

    createTicket: async (data: CreateTicketDto): Promise<TicketDto> => {
        const response = await ImsApi.post<TicketDto>('/tickets', data);
        return response.data;
    },

    getTicketById: async (id: string): Promise<TicketDto> => {
        const response = await ImsApi.get<TicketDto>(`/tickets/${id}`);
        return response.data;
    },

    updateTicket: async (id: string, data: UpdateTicketDto): Promise<TicketDto> => {
        const response = await ImsApi.put<TicketDto>(`/tickets/${id}`, data);
        return response.data;
    },

    getTicketsByBoard: async (boardId: string): Promise<TicketDto[]> => {
        const response = await ImsApi.get<TicketDto[]>(`/tickets/by-board/${boardId}`);
        return response.data;
    }
};
