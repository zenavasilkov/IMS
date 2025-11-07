using Application.Abstractions.Messaging;
using Pagination;

namespace Application.Interviews.Queries.GetInterviewsByCandidateId;

public sealed record GetInterviewsByCandidateIdQuery(
    Guid CandidateId,
    PaginationParameters PaginationParameters)
    : IQuery<GetInterviewsByCandidateIdQueryResponse>;
