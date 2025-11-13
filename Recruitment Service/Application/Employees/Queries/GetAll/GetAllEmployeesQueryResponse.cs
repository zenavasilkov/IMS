using Application.Employees.Queries.GetEmployeeById;
using Pagination;

namespace Application.Employees.Queries.GetAll;

public sealed record GetAllEmployeesQueryResponse(PagedList<GetEmployeeByIdQueryResponse> Employees);
