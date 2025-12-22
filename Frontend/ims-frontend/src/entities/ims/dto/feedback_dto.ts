export interface FeedbackDto {
    id: string;
    ticketId: string;
    sentById: string;
    addressedToId: string;
    comment?: string | null;
}

export interface CreateFeedbackDto {
    ticketId: string;
    sentById: string;
    addressedToId: string;
    comment?: string | null;
}

export interface UpdateFeedbackDto {
    comment?: string | null;
}
