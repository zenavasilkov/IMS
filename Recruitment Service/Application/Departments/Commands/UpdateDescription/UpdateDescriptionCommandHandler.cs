using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Shared;
using static Application.Errors.ApplicationErrors;

namespace Application.Departments.Commands.UpdateDescription;

public class UpdateDescriptionCommandHandler(IDepartmentRepository repository) : ICommandHandler<UpdateDescriptionCommand>
{
    public async Task<Result> Handle(UpdateDescriptionCommand request, CancellationToken cancellationToken)
    {
        var existingDepartment = await repository.GetByIdAsync(request.Id, true, cancellationToken);

        if (existingDepartment is null) return DepartmentErrors.NotFound;

        var result = existingDepartment.UpdateDescription(request.NewDescription);

        if (result.IsFailure) return result.Error;

        await repository.UpdateAsync(existingDepartment, cancellationToken);

        return Result.Success();
    }
}
