import { ImsApi } from '../axios';
import type { BoardDto, CreateBoardDto, UpdateBoardDto } from '../../entities/ims/dto/board_dto';
import type {BoardDtoPagedList} from "../../entities/ims/Pagination.ts";
import type {FetchBoardsParams} from "../../entities/ims/FetchParameters.ts";

export const boardService = {
    getAllBoards: async (params: FetchBoardsParams): Promise<BoardDtoPagedList> => {
        const response = await ImsApi.get<BoardDtoPagedList>('/boards', { params: params });
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
