using Application.Abstractions.Messaging;

namespace Application.Candidates.Queries.GetCandidateCvUrlById;

public sealed record GetCandidateCvUrlByIdQuery(Guid CandidateId) : IQuery<string>;
