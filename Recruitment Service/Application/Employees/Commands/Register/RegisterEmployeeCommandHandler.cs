using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Entities;
using Domain.Shared;
using Domain.ValueObjects;
using Pagination;
using static Application.Errors.ApplicationErrors;

namespace Application.Employees.Commands.Register;

public class RegisterEmployeeCommandHandler(IGenericRepository<Employee> repository,
    IDepartmentRepository departmentRepository) : ICommandHandler<RegistedEmployeeCommand>
{
    public async Task<Result> Handle(RegistedEmployeeCommand request, CancellationToken cancellationToken)
    {
        var fullName = FullName.Create(request.FirstName, request.LastName, request.Patronymic);

        if (fullName.IsFailure) return fullName.Error;

        var department = await departmentRepository.GetByIdAsync(request.DepartmentId, false, cancellationToken);

        if (department is null) return DepartmentErrors.NotFound;

        var paginationParameters = new PaginationParameters(1, 1);

        var existingEmployee = await repository.GetByConditionAsync(e => 
            e.Email == request.Email, paginationParameters, false, cancellationToken);

        if (existingEmployee is not null) return EmployeeErrors.EmailIsNotUnique;

        var employee = Employee.Create(Guid.NewGuid(), fullName.Value, request.Role, request.Email, department);

        if (employee.IsFailure) return employee.Error;

        await repository.CreateAsync(employee.Value, cancellationToken);

        return Result.Success();
    }
}
