using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Shared;
using Mapster;
using static Application.Errors.ApplicationErrors;

namespace Application.Departments.Queries.GetDepartmentByName;

public class GetDepartmentByNameQueryHandler(IDepartmentRepository repository)
    : IQueryHandler<GetDepartmentByNameQuery, GetDepartmentByNameResponse>
{
    public async Task<Result<GetDepartmentByNameResponse>> Handle(GetDepartmentByNameQuery request, CancellationToken cancellationToken)
    {
        var department = await repository.GetByNameAsync(request.Name, false, cancellationToken);

        if (department is null) return DepartmentErrors.NotFound;

        var response = department.Adapt<GetDepartmentByNameResponse>();

        return response;
    }
}
