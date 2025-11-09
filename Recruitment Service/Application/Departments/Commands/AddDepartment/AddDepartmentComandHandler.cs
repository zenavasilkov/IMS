using Application.Abstractions.Messaging;
using static Application.Errors.ApplicationErrors;
using Domain.Contracts.Repositories;
using Domain.Shared;
using Domain.Entities;

namespace Application.Departments.Commands.AddDepartment;

public class AddDepartmentComandHandler(IDepartmentRepository repository) : ICommandHandler<AddDepartmentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
    {
        var existingDepartment = await repository.GetByNameAsync(request.Name, false, cancellationToken);

        if (existingDepartment is not null) return DepartmentErrors.NameIsNotUnique;

        var newDepartment = Department.Create(Guid.NewGuid(), request.Name, request.Description);

        if (newDepartment.IsFailure) return newDepartment.Error;

        await repository.CreateAsync(newDepartment.Value, cancellationToken);

        return Result.Success(newDepartment.Value.Id);
    }
}
