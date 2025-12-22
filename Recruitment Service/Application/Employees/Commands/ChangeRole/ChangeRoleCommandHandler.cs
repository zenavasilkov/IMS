using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Entities;
using Domain.Shared;
using static Application.Errors.ApplicationErrors;

namespace Application.Employees.Commands.ChangeRole;

public class ChangeRoleCommandHandler(IGenericRepository<Employee> repository) : ICommandHandler<ChangeRoleCommand>
{
    public async Task<Result> Handle(ChangeRoleCommand request, CancellationToken cancellationToken)
    {
        var employee = await repository.GetByIdAsync(request.EmployeeId, true, cancellationToken);

        if (employee is null) return EmployeeErrors.NotFound;

        var result = employee.ChangeRole(request.NewRole);

        if (result.IsFailure) return result.Error;

        await repository.UpdateAsync(employee, cancellationToken);

        return Result.Success();
    }
}
