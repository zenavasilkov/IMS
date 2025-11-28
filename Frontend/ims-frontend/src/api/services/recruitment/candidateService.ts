import { RecruitmentApi } from '../../axios';
import type {
    RegisterCandidateCommand,
    FindCandidateByEmailQueryResponse,
    FindCandidateByIdQueryResponse,
    GetAllCandidatesQueryResponse,
    UpdateCvLinkCommand
} from '../../../entities/recruitment/dto/candidate_dto';

export const candidateService = {
    registerCandidate: async (data: RegisterCandidateCommand): Promise<FindCandidateByIdQueryResponse> => {
        const response = await RecruitmentApi.post<FindCandidateByIdQueryResponse>('/candidates/register', data);
        return response.data;
    },

    acceptToInternship: async (candidateId: string): Promise<void> => {
        await RecruitmentApi.put(`/candidates/accept-to-internship`, { candidateId });
    },

    updateCvLink: async (data: UpdateCvLinkCommand): Promise<FindCandidateByIdQueryResponse> => {
        const response = await RecruitmentApi.put<FindCandidateByIdQueryResponse>('/candidates/update-cv-link', data);
        return response.data;
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
            params: { pageNumber, pageSize }
        });
        return response.data;
    }
};
