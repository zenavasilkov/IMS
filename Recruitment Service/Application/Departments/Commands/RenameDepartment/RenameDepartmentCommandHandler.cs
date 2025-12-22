using static Application.Errors.ApplicationErrors;
using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Shared;

namespace Application.Departments.Commands.RenameDepartment;

public class RenameDepartmentCommandHandler(IDepartmentRepository repository) : ICommandHandler<RenameDepartmentCommand>
{
    public async Task<Result> Handle(RenameDepartmentCommand request, CancellationToken cancellationToken)
    {
        var existingDepartment = await repository.GetByNameAsync(request.NewName, false, cancellationToken);

        if (existingDepartment is not null && existingDepartment.Id != request.Id) return DepartmentErrors.NameIsNotUnique;

        existingDepartment = await repository.GetByIdAsync(request.Id, true, cancellationToken);

        if (existingDepartment is null) return DepartmentErrors.NotFound;

        var result = existingDepartment.Rename(request.NewName);
        
        if (result.IsFailure) return result.Error;

        await repository.UpdateAsync(existingDepartment, cancellationToken);

        return Result.Success();
    }
}
