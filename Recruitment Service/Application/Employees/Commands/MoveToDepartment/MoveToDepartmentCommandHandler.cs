using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Entities;
using Domain.Shared;
using static Application.Errors.ApplicationErrors;

namespace Application.Employees.Commands.MoveToDepartment;

public class MoveToDepartmentCommandHandler(IGenericRepository<Employee> repository,
    IDepartmentRepository departmentRepository) : ICommandHandler<MoveToDepartmentCommand>
{
    public async Task<Result> Handle(MoveToDepartmentCommand request, CancellationToken cancellationToken)
    {
        var employee = await repository.GetByIdAsync(request.EmployeeId, true, cancellationToken);

        if (employee is null) return EmployeeErrors.NotFound;

        var department = await departmentRepository.GetByIdAsync(request.DepartmentId, false, cancellationToken);

        if (department is null) return DepartmentErrors.NotFound;

        var result = employee.MoveTo(department);

        if (result.IsFailure) return result.Error;

        await repository.UpdateAsync(employee, cancellationToken);

        return Result.Success();
    }
}
