using Application.Abstractions.Messaging;

namespace Application.Candidates.Queries.FindByEmail;

public sealed record FindCandidateByEmailQuery(string Email) : IQuery<FindCandidateByEmailQueryResponse>;
