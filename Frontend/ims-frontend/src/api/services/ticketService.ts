import { ImsApi } from '../axios';
import type { TicketDto, CreateTicketDto, UpdateTicketDto } from '../../entities/ims/dto/ticket_dto';
import type {FetchTicketsParams} from "../../entities/ims/FetchParameters.ts";
import type {TicketDtoPagedList} from "../../entities/ims/Pagination.ts";

export const ticketService = {
    getAllTickets: async (params: FetchTicketsParams): Promise<TicketDtoPagedList> => {
        const response = await ImsApi.get<TicketDtoPagedList>('/tickets', { params: params });
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
};
