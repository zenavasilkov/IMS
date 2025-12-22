using Application.Departments.Queries.GetDepartmentById;
using Pagination;

namespace Application.Departments.Queries.GetAll;

public sealed record GetAllDepartmentsQueryResponse(PagedList<GetDepartmentByIdQueryResponse> Departments);
