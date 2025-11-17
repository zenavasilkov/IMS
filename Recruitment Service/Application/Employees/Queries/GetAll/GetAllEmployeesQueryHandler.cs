using Application.Abstractions.Messaging;
using Application.Employees.Queries.GetEmployeeById;
using Domain.Contracts.Repositories;
using Domain.Shared;
using Mapster;
using Pagination;

namespace Application.Employees.Queries.GetAll;

public class GetAllEmployeesQueryHandler(IEmployeeRepository repository) : IQueryHandler<GetAllEmployeesQuery, GetAllEmployeesQueryResponse>
{
    public async Task<Result<GetAllEmployeesQueryResponse>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await repository.GetByConditionAsync(e => true, request.PaginationParameters, false, cancellationToken);

        var pagedList = employees.Adapt<PagedList<GetEmployeeByIdQueryResponse>>();

        var response = new GetAllEmployeesQueryResponse(pagedList);

        return response;
    }
}
