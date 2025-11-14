using Application.Abstractions.Messaging;
using Application.Employees.Queries.GetEmployeeById;
using Domain.Contracts.Repositories;
using Domain.Entities;
using Domain.Shared;
using Pagination;

namespace Application.Employees.Queries.GetAll;

public class GetAllEmployeesQueryHandler(IEmployeeRepository repository) : IQueryHandler<GetAllEmployeesQuery, GetAllEmployeesQueryResponse>
{
    public async Task<Result<GetAllEmployeesQueryResponse>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await repository.GetByConditionAsync(e => true, request.PaginationParameters, false, cancellationToken);

        var list = employees.Items.Select(Map).ToList();

        var pagedList = new PagedList<GetEmployeeByIdQueryResponse>(list, employees.PageNumber, employees.PageSize, employees.TotalCount);

        var response = new GetAllEmployeesQueryResponse(pagedList);

        return response;
    }

    private static GetEmployeeByIdQueryResponse Map(Employee e) => new(
            e.Id,
            e.FullName.FirstName,
            e.FullName.LastName,
            e.FullName.Patronymic,
            e.Role,
            e.Email,
            e.DepartmentId);
}
