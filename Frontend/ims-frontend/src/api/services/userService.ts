import { ImsApi } from '../axios';
import type { UserDto, CreateUserDto, UpdateUserDto, UserDtoPagedList } from '../../entities/ims/dto/user_dto';

export const userService = {
    getAllUsers: async (pageNumber = 1, pageSize = 10): Promise<UserDtoPagedList> => {
        const response = await ImsApi.get<UserDtoPagedList>('users', {
            params: { pageNumber, pageSize }
        });
        return response.data;
    },

    createUser: async (data: CreateUserDto): Promise<UserDto> => {
        const response = await ImsApi.post<UserDto>('/users', data);
        return response.data;
    },

    getUserById: async (id: string): Promise<UserDto> => {
        const response = await ImsApi.get<UserDto>(`/users/${id}`);
        return response.data;
    },

    updateUser: async (id: string, data: UpdateUserDto): Promise<UserDto> => {
        const response = await ImsApi.put<UserDto>(`/users/${id}`, data);
        return response.data;
    }
};
