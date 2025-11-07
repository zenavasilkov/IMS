using Application.Abstractions.Messaging;
using Application.Interviews.Queries.GetInterviewById;
using Domain.Contracts.Repositories;
using Domain.Entities;
using Domain.Shared;
using Pagination;
using static Application.Errors.ApplicationErrors;

namespace Application.Interviews.Queries.GetInterviewsByCandidateId;

public class GetInterviewsByCandidateIdQueryHandler(
    IGenericReadOnlyRepository<Interview> repository,
    IGenericReadOnlyRepository<Candidate> candidateRepository,
    IGenericReadOnlyRepository<Employee> employeeRepository,
    IGenericRepository<Department> departmentRepository) : IQueryHandler<GetInterviewsByCandidateIdQuery, GetInterviewsByCandidateIdQueryResponse>
{
    public async Task<Result<GetInterviewsByCandidateIdQueryResponse>> Handle(GetInterviewsByCandidateIdQuery request, CancellationToken cancellationToken)
    {
        var interviews = await repository.GetByConditionAsync(i => 
            i.CandidateId == request.CandidateId, request.PaginationParameters, false, cancellationToken);

        var items = new List<GetInterviewByIdQueryResponse>();


        foreach (var interview in interviews.Items)
        {
            if (interview is null) return InterviewErrors.NotFound;

            var interviewer = await employeeRepository.GetByIdAsync(interview.InterviewerId, false, cancellationToken);
            var candidate = await candidateRepository.GetByIdAsync(interview.CandidateId, false, cancellationToken);
            var department = await departmentRepository.GetByIdAsync(interview.DepartmentId, false, cancellationToken);

            var item = new GetInterviewByIdQueryResponse(
                interview.Id,
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

            items.Add(item);
        }

        var PagedItems = new PagedList<GetInterviewByIdQueryResponse>(
            items,
            request.PaginationParameters.PageNumber,
            request.PaginationParameters.PageSize,
            interviews.TotalCount);

        var response = new GetInterviewsByCandidateIdQueryResponse(PagedItems);

        return response;
    }
}
