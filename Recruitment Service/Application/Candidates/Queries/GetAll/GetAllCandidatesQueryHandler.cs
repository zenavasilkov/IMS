using Application.Abstractions.Messaging;
using Application.Candidates.Queries.FindById;
using Domain.Contracts.Repositories;
using Domain.Entities;
using Domain.Shared;
using Pagination;
using System.Data;

namespace Application.Candidates.Queries.GetAll;

public class GetAllCandidatesQueryHandler(ICandidateRepository repository) : IQueryHandler<GetAllCandidatesQuery, GetAllCandidatesQueryResponce>
{
    public async Task<Result<GetAllCandidatesQueryResponce>> Handle(GetAllCandidatesQuery request, CancellationToken cancellationToken)
    {
        var candidates = await repository.GetByConditionAsync(c => true, request.PaginationParameters, false, cancellationToken);

        var list = candidates.Items.Select(Map).ToList();

        var pagedList = new PagedList<FindCandidateByIdQueryResponse>(list, candidates.PageNumber, candidates.PageSize, candidates.TotalCount);

        var response = new GetAllCandidatesQueryResponce(pagedList);

        return response;
    }

    private static FindCandidateByIdQueryResponse Map(Candidate c) =>
        new(
            c.Id,
            c.FullName.FirstName,
            c.FullName.LastName,
            c.Email,
            c.IsAcceptedToInternship,
            c.PhoneNumber,
            c.CvLink,
            c.LinkedIn,
            c.FullName.Patronymic
        );
}
