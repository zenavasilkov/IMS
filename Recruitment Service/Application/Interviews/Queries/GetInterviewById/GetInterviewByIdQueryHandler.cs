using Application.Abstractions.Messaging;
using Domain.Contracts.Repositories;
using Domain.Entities;
using Domain.Shared;
using static Application.Errors.ApplicationErrors;

namespace Application.Interviews.Queries.GetInterviewById;

public class GetInterviewByIdQueryHandler(
    IGenericReadOnlyRepository<Interview> repository,
    IGenericReadOnlyRepository<Candidate> candidateRepository,
    IGenericReadOnlyRepository<Employee> employeeRepository,
    IGenericRepository<Department> departmentRepository)
    : IQueryHandler<GetInterviewByIdQuery, GetInterviewByIdQueryResponse>
{
    public async Task<Result<GetInterviewByIdQueryResponse>> Handle(GetInterviewByIdQuery request, CancellationToken cancellationToken)
    {
        var interview = await repository.GetByIdAsync(request.Id, false, cancellationToken);

        if (interview is null) return InterviewErrors.NotFound;

        var candidate = await candidateRepository.GetByIdAsync(interview.CandidateId, false, cancellationToken);
        var interviewer = await employeeRepository.GetByIdAsync(interview.InterviewerId, false, cancellationToken);
        var department = await departmentRepository.GetByIdAsync(interview.DepartmentId, false, cancellationToken);

        var response = new GetInterviewByIdQueryResponse(
            request.Id,
            interview.CandidateId,
            interview.InterviewerId,
            interview.DepartmentId,
            candidate!.Email,
            interviewer!.Email,
            department!.Name,
            interview.Type,
            interview.ScheduledAt,
            interview.Feedback,
            interview.IsPassed,
            interview.IsCancelled);

        return response;
    }
}
