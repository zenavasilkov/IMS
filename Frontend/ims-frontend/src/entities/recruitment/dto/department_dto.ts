export interface AddDepartmentCommand {
    name?: string | null;
    description?: string | null;
}

export interface RenameDepartmentCommand {
    id: string;
    newName?: string | null;
}

export interface UpdateDescriptionCommand {
    id: string;
    newDescription?: string | null;
}

export interface GetDepartmentByIdQueryResponse {
    id: string;
    name?: string | null;
    description?: string | null;
}

export interface GetDepartmentByIdQueryResponsePagedList {
    items: GetDepartmentByIdQueryResponse[] | null;
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    hasPrevious: boolean;
    hasNext: boolean;
}

export interface GetAllDepartmentsQueryResponse {
  departments: GetDepartmentByIdQueryResponsePagedList;
}

export interface GetDepartmentByNameResponse {
    id: string;
    name?: string | null;
    description?: string | null;
}

