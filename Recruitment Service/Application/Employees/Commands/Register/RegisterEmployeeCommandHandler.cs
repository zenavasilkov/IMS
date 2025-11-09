using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Entities;
using Domain.Shared;
using Domain.ValueObjects;
using Pagination;
using static Application.Errors.ApplicationErrors;

namespace Application.Employees.Commands.Register;

public class RegisterEmployeeCommandHandler(IEmployeeRepository repository,
    IDepartmentRepository departmentRepository) : ICommandHandler<RegisterEmployeeCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterEmployeeCommand request, CancellationToken cancellationToken)
    {
        var fullName = FullName.Create(request.FirstName, request.LastName, request.Patronymic);

        if (fullName.IsFailure) return fullName.Error;

        var department = await departmentRepository.GetByIdAsync(request.DepartmentId, false, cancellationToken);

        if (department is null) return DepartmentErrors.NotFound;

        var existingEmployee = await repository.GetByEmailAsync(request.Email, false, cancellationToken);

        if (existingEmployee is not null) return EmployeeErrors.EmailIsNotUnique;

        var employee = Employee.Create(Guid.NewGuid(), fullName.Value, request.Role, request.Email, department);

        if (employee.IsFailure) return employee.Error;

        await repository.CreateAsync(employee.Value, cancellationToken);

        return Result.Success(employee.Value.Id);
    }
}
