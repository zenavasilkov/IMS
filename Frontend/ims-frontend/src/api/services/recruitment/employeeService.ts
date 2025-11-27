import { RecruinmentApi } from '../../axios';
import type {
    ChangeRoleCommand,
    RegisterEmployeeCommand,
    GetEmployeeByIdQueryResponse,
    GetAllEmployeesQueryResponse,
    MoveToDepartmentCommand
} from '../../../entities/recruitment/dto/employee_dto';

export const employeeService = {
    registerEmployee: async (data: RegisterEmployeeCommand): Promise<GetEmployeeByIdQueryResponse> => {
        const response = await RecruinmentApi.post<GetEmployeeByIdQueryResponse>('/employees', data);
        return response.data;
    },

    changeRole: async (data: ChangeRoleCommand): Promise<GetEmployeeByIdQueryResponse> => {
        const response = await RecruinmentApi.put<GetEmployeeByIdQueryResponse>('/employees/change-role', data);
        return response.data;
    },

    moveToDepartment: async (data: MoveToDepartmentCommand): Promise<GetEmployeeByIdQueryResponse> => {
        const response = await RecruinmentApi.put<GetEmployeeByIdQueryResponse>('/employees/move-to-department', data);
        return response.data;
    },

    getEmployeeById: async (id: string): Promise<GetEmployeeByIdQueryResponse> => {
        const response = await RecruinmentApi.get<GetEmployeeByIdQueryResponse>(`/employees/${id}`);
        return response.data;
    },

    getAllEmployees: async (pageNumber = 1, pageSize = 10): Promise<GetAllEmployeesQueryResponse> => {
        const response = await RecruinmentApi.get<GetAllEmployeesQueryResponse>('/employees/get-all', {
            params: {
                'PaginationParameters.PageNumber': pageNumber,
                'PaginationParameters.PageSize': pageSize
            }
        });
        return response.data;
    }
};
