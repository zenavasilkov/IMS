import { Role } from '../enums';

export interface CreateUserDto {
    email?: string;
    phoneNumber?: string;
    firstname?: string;
    lastname?: string;
    patronymic?: string;
    role: Role;
}

export interface UserDto {
    id: string;
    email?: string | null;
    phoneNumber?: string | null;
    firstname?: string | null;
    lastname?: string | null;
    patronymic?: string | null;
    role: Role;
}

export interface UpdateUserDto {
    email?: string | null;
    phoneNumber?: string | null;
    firstname?: string | null;
    lastname?: string | null;
    patronymic?: string | null;
    role?: Role;
}

export interface UserDtoPagedList {
    items: UserDto[] | null;
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    hasPrevious: boolean;
    hasNext: boolean;
}
