using Application.Abstractions.Messaging;
using Pagination;

namespace Application.Candidates.Queries.GetAll;

public sealed record GetAllCandidatesQuery(PaginationParameters PaginationParameters) : IQuery<GetAllCandidatesQueryResponce>;
