using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Entities;
using Domain.Shared;
using Mapster;
using static Application.Errors.ApplicationErrors;

namespace Application.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdCommandHandler(IGenericReadOnlyRepository<Employee> repository)
    : IQueryHandler<GetEmployeeByIdQuery, GetEmployeeByIdQueryResponse>
{
    public async Task<Result<GetEmployeeByIdQueryResponse>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await repository.GetByIdAsync(request.Id, false, cancellationToken);

        if (employee is null) return EmployeeErrors.NotFound;

        var response = employee.Adapt<GetEmployeeByIdQueryResponse>();

        return response;
    }
}
