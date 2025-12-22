import { ImsApi } from '../axios';
import type { InternshipDto, CreateInternshipDto, UpdateInternshipDto } from '../../entities/ims/dto/internship_dto';
import type {FetchInternshipsParams} from "../../entities/ims/FetchParameters.ts";
import type {InternshipDtoPagedList} from "../../entities/ims/Pagination.ts";

export const internshipService = {
    getAllInternships: async (params: FetchInternshipsParams): Promise<InternshipDtoPagedList> => {
        const response = await ImsApi.get<InternshipDtoPagedList>('/internships', { params: params });
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
};
