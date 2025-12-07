import { ImsApi } from '../axios';
import type { UserDto, CreateUserDto, UpdateUserDto, UserDtoPagedList } from '../../entities/ims/dto/user_dto';
import type { FetchUsersParams } from '../../features/userManagement/userManagementSlice';

export const userService = {
    getAllUsers: async (params: FetchUsersParams): Promise<UserDtoPagedList> => {
        const response = await ImsApi.get<UserDtoPagedList>('users', {
            params: params 
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
