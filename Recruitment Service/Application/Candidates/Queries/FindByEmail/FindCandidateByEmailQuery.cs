using Application.Abstractions.Messaging;
using Domain.Entities;

namespace Application.Candidates.Queries.FindByEmail;

public sealed record FindCandidateByEmailQuery(string Email) : IQuery<FindCandidateByEmailQueryResponse>;
