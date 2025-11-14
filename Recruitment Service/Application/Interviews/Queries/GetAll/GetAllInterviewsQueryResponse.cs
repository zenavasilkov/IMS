using Application.Interviews.Queries.GetInterviewById;
using Pagination;

namespace Application.Interviews.Queries.GetAll;

public sealed record GetAllInterviewsQueryResponse(PagedList<GetInterviewByIdQueryResponse> Interviews);
