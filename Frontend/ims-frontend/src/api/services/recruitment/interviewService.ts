import { RecruinmentApi } from '../../axios';
import type {
    GetInterviewByIdQueryResponse,
    GetAllInterviewsQueryResponse,
    GetInterviewsByCandidateIdQueryResponse,
    ScheduleInterviewCommand,
    RescheduleInterviewCommand,
    AddFeedbackCommand
} from '../../../entities/recruitment/dto/interview_dto';

export const interviewService = {
    scheduleInterview: async (data: ScheduleInterviewCommand): Promise<GetInterviewByIdQueryResponse> => {
        const response = await RecruinmentApi.post<GetInterviewByIdQueryResponse>('/interviews', data);
        return response.data;
    },

    rescheduleInterview: async (data: RescheduleInterviewCommand): Promise<GetInterviewByIdQueryResponse> => {
        const response = await RecruinmentApi.put<GetInterviewByIdQueryResponse>('/interviews/reschedule', data);
        return response.data;
    },

    cancelInterview: async (interviewId: string): Promise<void> => {
        await RecruinmentApi.put(`/interviews/cancel`, { interviewId });
    },

    addFeedback: async (data: AddFeedbackCommand): Promise<GetInterviewByIdQueryResponse> => {
        const response = await RecruinmentApi.put<GetInterviewByIdQueryResponse>('/interviews/add-feedback', data);
        return response.data;
    },

    getInterviewById: async (id: string): Promise<GetInterviewByIdQueryResponse> => {
        const response = await RecruinmentApi.get<GetInterviewByIdQueryResponse>(`/interviews/${id}`);
        return response.data;
    },

    getInterviewsByCandidate: async (candidateId: string, pageNumber = 1, pageSize = 10): Promise<GetInterviewsByCandidateIdQueryResponse> => {
        const response = await RecruinmentApi.get<GetInterviewsByCandidateIdQueryResponse>('/interviews/by-candidate', {
            params: { candidateId, pageNumber, pageSize }
        });
        return response.data;
    },

    getAllInterviews: async (pageNumber = 1, pageSize = 10): Promise<GetAllInterviewsQueryResponse> => {
        const response = await RecruinmentApi.get<GetAllInterviewsQueryResponse>('/interviews/get-all', {
            params: { pageNumber, pageSize }
        });
        return response.data;
    }
};
