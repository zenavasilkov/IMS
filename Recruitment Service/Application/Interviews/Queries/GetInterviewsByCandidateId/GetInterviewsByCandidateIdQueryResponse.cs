using Application.Interviews.Queries.GetInterviewById;
using Pagination;

namespace Application.Interviews.Queries.GetInterviewsByCandidateId;

public sealed record GetInterviewsByCandidateIdQueryResponse(PagedList<GetInterviewByIdQueryResponse> Interviews);
