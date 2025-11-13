using Application.Candidates.Queries.FindById;
using Pagination;

namespace Application.Candidates.Queries.GetAll;

public sealed record GetAllCandidatesQueryResponce(PagedList<FindCandidateByIdQueryResponse> Candidates);
