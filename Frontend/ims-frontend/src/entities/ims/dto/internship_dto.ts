import { InternshipStatus } from "../enums";

export interface InternshipDto {
    id: string;
    internId: string;
    mentorId: string;
    humanResourcesManagerId: string;
    startDate: string;
    endDate?: string | null;
    status: InternshipStatus;
}

export interface CreateInternshipDto {
    internId: string;
    mentorId: string;
    humanResourcesManagerId: string;
    startDate: string;
    endDate?: string | null;
    status: InternshipStatus;
}

export interface UpdateInternshipDto {
    mentorId: string;
    humanResourcesManagerId: string;
    startDate: string;
    endDate?: string | null;
    status: InternshipStatus;
}
