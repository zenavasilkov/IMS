using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Shared;
using static Application.Errors.ApplicationErrors;

namespace Application.Departments.Queries.GetDepartmentById;

public class GetDepartmentByIdQueryHandler(IDepartmentRepository repository) : IQueryHandler<GetDepartmentByIdQuery, GetDepartmentByIdQueryResponse>
{
    public async Task<Result<GetDepartmentByIdQueryResponse>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var department = await repository.GetByIdAsync(request.Id, false, cancellationToken);

        if (department is null) return DepartmentErrors.NotFound;

        var response = new GetDepartmentByIdQueryResponse(department.Id, department.Name, department.Description);

        return response;
    }
}
