import { RecruitmentApi } from '../../axios';
import type {
    AddDepartmentCommand,
    RenameDepartmentCommand,
    UpdateDescriptionCommand,
    GetDepartmentByIdQueryResponse,
    GetAllDepartmentsQueryResponse,
    GetDepartmentByNameResponse
} from '../../../entities/recruitment/dto/department_dto';

export const departmentService = {
    addDepartment: async (data: AddDepartmentCommand): Promise<GetDepartmentByIdQueryResponse> => {
        const response = await RecruitmentApi.post<GetDepartmentByIdQueryResponse>('/departments', data);
        return response.data;
    },

    renameDepartment: async (data: RenameDepartmentCommand): Promise<GetDepartmentByIdQueryResponse> => {
        const response = await RecruitmentApi.put<GetDepartmentByIdQueryResponse>('/departments/rename', data);
        return response.data;
    },

    updateDescription: async (data: UpdateDescriptionCommand): Promise<GetDepartmentByIdQueryResponse> => {
        const response = await RecruitmentApi.put<GetDepartmentByIdQueryResponse>('/departments/update-description', data);
        return response.data;
    },

    getDepartmentById: async (id: string): Promise<GetDepartmentByIdQueryResponse> => {
        const response = await RecruitmentApi.get<GetDepartmentByIdQueryResponse>(`/departments/${id}`);
        return response.data;
    },

    getDepartmentByName: async (name: string): Promise<GetDepartmentByNameResponse> => {
        const response = await RecruitmentApi.get<GetDepartmentByNameResponse>(`/departments/by-name/${name}`);
        return response.data;
    },

    getAllDepartments: async (pageNumber = 1, pageSize = 10): Promise<GetAllDepartmentsQueryResponse> => {
        const response = await RecruitmentApi.get<GetAllDepartmentsQueryResponse>('/departments/get-all', {
            params: { pageNumber, pageSize }
        });
        return response.data;
    }
};
