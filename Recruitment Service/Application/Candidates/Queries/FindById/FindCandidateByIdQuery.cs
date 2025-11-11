using Application.Abstractions.Messaging;

namespace Application.Candidates.Queries.FindById;

public sealed record FindCandidateByIdQuery(Guid Id) : IQuery<FindCandidateByIdQueryResponse>;
