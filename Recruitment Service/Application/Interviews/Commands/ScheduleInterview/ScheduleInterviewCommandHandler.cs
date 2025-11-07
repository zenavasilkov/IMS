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
    : ICommandHandler<ScheduleInterviewCommand>
{
    public async Task<Result> Handle(ScheduleInterviewCommand request, CancellationToken cancellationToken)
    {
        var candidate = await candidateRepository.GetByIdAsync(request.CandidateId, false, cancellationToken);

        if (candidate is null) return CandidateErrors.NotFound;

        var interviewer = await employeeRepository.GetByIdAsync(request.InterviewerId, false, cancellationToken);

        if (interviewer is null) return EmployeeErrors.NotFound;

        var department = await departmentRepository.GetByIdAsync(request.DepartmentId, false, cancellationToken);

        if (department is null) return DepartmentErrors.NotFound;

        var interview = Interview.Create(Guid.NewGuid(), candidate, interviewer, department, request.Type, request.ScheduledAt);

        if (interview.IsFailure) return interview.Error;

        await repository.CreateAsync(interview.Value, cancellationToken);

        return Result.Success();
    }
}
