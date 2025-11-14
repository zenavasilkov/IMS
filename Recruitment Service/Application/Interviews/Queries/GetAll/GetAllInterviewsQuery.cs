using Application.Abstractions.Messaging;
using Pagination;

namespace Application.Interviews.Queries.GetAll;

public sealed record GetAllInterviewsQuery(PaginationParameters PaginationParameters) : IQuery<GetAllInterviewsQueryResponse>;
