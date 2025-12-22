export interface BoardDto {
    id: string;
    title?: string | null;
    description?: string | null;
    createdById: string;
    createdToId: string;
}

export interface CreateBoardDto {
    createdById: string
    createdToId: string;
    title?: string | null;
    description?: string | null;
}

export interface UpdateBoardDto {
    title?: string | null;
    description?: string | null;
}
