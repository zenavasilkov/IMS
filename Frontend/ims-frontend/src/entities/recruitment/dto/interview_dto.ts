import { InterviewType } from "../enums";

export interface GetInterviewByIdQueryResponse {
    id: string;
    candidateId: string;
    interviewerId: string;
    departmentId: string;
    candidateEmail?: string | null;
    interviewerEmail?: string | null;
    deparnmentName?: string | null;
    interviewType: InterviewType;
    scheduledAt: string;
    feedback?: string | null;
    isPassed: boolean;
    isCancelled: boolean;
}

export interface GetInterviewByIdQueryResponsePagedList {
    items: GetInterviewByIdQueryResponse[] | null;
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    hasPrevious: boolean;
    hasNext: boolean;
}

export interface GetAllInterviewsQueryResponse {
    interviews: GetInterviewByIdQueryResponsePagedList;
}

export interface GetInterviewsByCandidateIdQueryResponse {
    interviews: GetInterviewByIdQueryResponsePagedList;
}

export interface ScheduleInterviewCommand {
    candidateId: string;
    interviewerId: string;
    departmentId: string;
    type: InterviewType;
    scheduledAt: string;
}

export interface RescheduleInterviewCommand {
    id: string;
    newDate: string;
}

export interface AddFeedbackCommand {
    id: string;
    feedback?: string | null;
    isPassed: boolean;
}
