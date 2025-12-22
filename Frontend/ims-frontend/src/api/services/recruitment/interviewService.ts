import { RecruitmentApi } from '../../axios';
import type {
    GetInterviewByIdQueryResponse,
    GetAllInterviewsQueryResponse,
    GetInterviewsByCandidateIdQueryResponse,
    ScheduleInterviewCommand,
    RescheduleInterviewCommand,
    AddFeedbackCommand
} from '../../../entities/recruitment/dto/interview_dto';

export const interviewService = {
    scheduleInterview: async (data: ScheduleInterviewCommand): Promise<string> => {
        const response = await RecruitmentApi.post<string>('/interviews', data);
        return response.data;
    },

    rescheduleInterview: async (data: RescheduleInterviewCommand): Promise<GetInterviewByIdQueryResponse> => {
        const response = await RecruitmentApi.put<GetInterviewByIdQueryResponse>('/interviews/reschedule', data);
        return response.data;
    },

    cancelInterview: async (interviewId: string): Promise<void> => await RecruitmentApi.put(`/interviews/cancel`, {}, { params: { Id: interviewId }}),

    addFeedback: async (data: AddFeedbackCommand): Promise<void> => await RecruitmentApi.put('/interviews/add-feedback', data),

    getInterviewById: async (id: string): Promise<GetInterviewByIdQueryResponse> => {
        const response = await RecruitmentApi.get<GetInterviewByIdQueryResponse>(`/interviews/${id}`);
        return response.data;
    },

    getInterviewsByCandidate: async (candidateId: string, pageNumber = 1, pageSize = 10): Promise<GetInterviewsByCandidateIdQueryResponse> => {
        const response = await RecruitmentApi.get<GetInterviewsByCandidateIdQueryResponse>('/interviews/by-candidate', {
            params: {
                candidateId : candidateId,
                PageNumber : pageNumber,
                PageSize: pageSize }
        });
        return response.data;
    },

    getAllInterviews: async (pageNumber = 1, pageSize = 10): Promise<GetAllInterviewsQueryResponse> => {
        const response = await RecruitmentApi.get<GetAllInterviewsQueryResponse>('/interviews/get-all', {
            params: {
                'PaginationParameters.PageNumber': pageNumber,
                'PaginationParameters.PageSize': pageSize
            }
        });
        return response.data;
    }
};
