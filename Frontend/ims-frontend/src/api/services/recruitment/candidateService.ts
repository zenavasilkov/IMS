import { RecruitmentApi } from '../../axios';
import type {
    RegisterCandidateCommand,
    FindCandidateByEmailQueryResponse,
    FindCandidateByIdQueryResponse,
    GetAllCandidatesQueryResponse
} from '../../../entities/recruitment/dto/candidate_dto';

export const candidateService = {
    registerCandidate: async (data: RegisterCandidateCommand): Promise<FindCandidateByIdQueryResponse> => {
        const response = await RecruitmentApi.post<FindCandidateByIdQueryResponse>('/candidates/register', data);
        return response.data;
    },

    acceptToInternship: async (candidateId: string): Promise<void> => {
        await RecruitmentApi.put(`/candidates/accept-to-internship`, {}, { params: { Id: candidateId } });
    },

    uploadCv: async (candidateId: string, file: File): Promise<void> => {
        const formData = new FormData();
        formData.append('file', file);
        await RecruitmentApi.put(`/candidates/${candidateId}/cv`, formData, {headers: {'Content-Type': 'multipart/form-data'}});
    },

    getCvViewUrl: async (candidateId: string): Promise<string> => {
        const response = await RecruitmentApi.get<{ url: string } | string>(`/candidates/${candidateId}/cv`);
        if (typeof response.data === 'string') return response.data;
        // @ts-ignore
        return response.data.url || response.data;
    },

    getCandidateByEmail: async (email: string): Promise<FindCandidateByEmailQueryResponse> => {
        const response = await RecruitmentApi.get<FindCandidateByEmailQueryResponse>(`/candidates/by-email/${email}`);
        return response.data;
    },

    getCandidateById: async (id: string): Promise<FindCandidateByIdQueryResponse> => {
        const response = await RecruitmentApi.get<FindCandidateByIdQueryResponse>(`/candidates/${id}`);
        return response.data;
    },

    getAllCandidates: async (pageNumber = 1, pageSize = 10): Promise<GetAllCandidatesQueryResponse> => {
        const response = await RecruitmentApi.get<GetAllCandidatesQueryResponse>('/candidates/get-all', {
            params: {
                'PaginationParameters.PageNumber': pageNumber,
                'PaginationParameters.PageSize': pageSize
            }
        });
        return response.data;
    }
};
