import { ImsApi } from '../axios';
import type { InternshipDto, CreateInternshipDto, UpdateInternshipDto } from '../../entities/ims/dto/internship_dto';

export const internshipService = {
    getAllInternships: async (): Promise<InternshipDto[]> => {
        const response = await ImsApi.get<InternshipDto[]>('/internships');
        return response.data;
    },

    createInternship: async (data: CreateInternshipDto): Promise<InternshipDto> => {
        const response = await ImsApi.post<InternshipDto>('/internships', data);
        return response.data;
    },

    getInternshipById: async (id: string): Promise<InternshipDto> => {
        const response = await ImsApi.get<InternshipDto>(`/internships/${id}`);
        return response.data;
    },

    updateInternship: async (id: string, data: UpdateInternshipDto): Promise<InternshipDto> => {
        const response = await ImsApi.put<InternshipDto>(`/internships/${id}`, data);
        return response.data;
    },

    getInternshipsByMentor: async (mentorId: string): Promise<InternshipDto[]> => {
        const response = await ImsApi.get<InternshipDto[]>(`/internships/by-mentor/${mentorId}`);
        return response.data;
    }
};
