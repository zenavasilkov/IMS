using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Entities;
using Domain.Shared;
using static Application.Errors.ApplicationErrors;

namespace Application.Interviews.Commands.ScheduleInterview;

public class ScheduleInterviewCommandHandler(
    IGenericRepository<Interview> repository,
    IDepartmentRepository departmentRepository,
    ICandidateRepository candidateRepository,
    IGenericReadOnlyRepository<Employee> employeeRepository)
    : ICommandHandler<ScheduleInterviewCommand, Guid>
{
    public async Task<Result<Guid>> Handle(ScheduleInterviewCommand request, CancellationToken cancellationToken)
    {
        var result = await ValidateDependencies(request, cancellationToken);
        if (result.IsFailure) return result.Error;
        var (candidate, interviewer, department) = result.Value;

        var interview = Interview.Create(Guid.NewGuid(), candidate, interviewer, department, request.Type, request.ScheduledAt);
        if (interview.IsFailure) return interview.Error;

        await repository.CreateAsync(interview.Value, cancellationToken);

        return Result.Success(interview.Value.Id);
    }

    private async Task<Result<(Candidate, Employee, Department)>> ValidateDependencies(
        ScheduleInterviewCommand request, CancellationToken cancellationToken)
    {
        var candidate = await candidateRepository.GetByIdAsync(request.CandidateId, true, cancellationToken);
        if (candidate is null) return CandidateErrors.NotFound;

        var interviewer = await employeeRepository.GetByIdAsync(request.InterviewerId, true, cancellationToken);
        if (interviewer is null) return EmployeeErrors.NotFound;

        var department = await departmentRepository.GetByIdAsync(request.DepartmentId, true, cancellationToken);
        if (department is null) return DepartmentErrors.NotFound;

        return Result.Success((candidate, interviewer, department));
    }
}
