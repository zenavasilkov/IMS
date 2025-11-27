import { EmployeeRole } from "../enums";

export interface ChangeRoleCommand {
  employeeId: string;
  newRole: EmployeeRole;
}

export interface RegisterEmployeeCommand {
  firstName?: string | null;
  lastName?: string | null;
  role: EmployeeRole;
  email?: string | null;
  departmentId: string;
  patronymic?: string | null;
}

export interface GetEmployeeByIdQueryResponse {
  id: string;
  firstName?: string | null;
  lastName?: string | null;
  patronymic?: string | null;
  role: EmployeeRole;
  email?: string | null;
  departmentId: string;
}

export interface GetEmployeeByIdQueryResponsePagedList {
  items: GetEmployeeByIdQueryResponse[] | null;
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export interface GetAllEmployeesQueryResponse {
  employees: GetEmployeeByIdQueryResponsePagedList;
}

export interface MoveToDepartmentCommand {
  employeeId: string;
  departmentId: string;
}
