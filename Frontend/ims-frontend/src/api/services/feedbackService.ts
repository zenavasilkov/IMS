import { ImsApi } from '../axios';
import type { FeedbackDto, CreateFeedbackDto, UpdateFeedbackDto } from '../../entities/ims/dto/feedback_dto';

export const feedbackService = {
    getAllFeedbacks: async (): Promise<FeedbackDto[]> => {
        const response = await ImsApi.get<FeedbackDto[]>('/feedbacks');
        return response.data;
    },

    createFeedback: async (data: CreateFeedbackDto): Promise<FeedbackDto> => {
        const response = await ImsApi.post<FeedbackDto>('/feedbacks', data);
        return response.data;
    },

    getFeedbackById: async (id: string): Promise<FeedbackDto> => {
        const response = await ImsApi.get<FeedbackDto>(`/feedbacks/${id}`);
        return response.data;
    },

    updateFeedback: async (id: string, data: UpdateFeedbackDto): Promise<FeedbackDto> => {
        const response = await ImsApi.put<FeedbackDto>(`/feedbacks/${id}`, data);
        return response.data;
    },

    getFeedbacksByTicket: async (ticketId: string): Promise<FeedbackDto[]> => {
        const response = await ImsApi.get<FeedbackDto[]>(`/feedbacks/by-ticket/${ticketId}`);
        return response.data;
    }
};
