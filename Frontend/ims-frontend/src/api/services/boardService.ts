import { ImsApi } from '../axios';
import type { BoardDto, CreateBoardDto, UpdateBoardDto } from '../../entities/ims/dto/board_dto';

export const boardService = {
    getAllBoards: async (): Promise<BoardDto[]> => {
        const response = await ImsApi.get<BoardDto[]>('/boards');
        return response.data;
    },

    createBoard: async (data: CreateBoardDto): Promise<BoardDto> => {
        const response = await ImsApi.post<BoardDto>('/boards', data);
        return response.data;
    },

    getBoardById: async (id: string): Promise<BoardDto> => {
        const response = await ImsApi.get<BoardDto>(`/boards/${id}`);
        return response.data;
    },

    updateBoard: async (id: string, data: UpdateBoardDto): Promise<BoardDto> => {
        const response = await ImsApi.put<BoardDto>(`/boards/${id}`, data);
        return response.data;
    }
};
