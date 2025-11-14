using Application.Abstractions.Messaging;
using Application.Departments.Queries.GetDepartmentById;
using Domain.Contracts.Repositories;
using Domain.Shared;
using Pagination;

namespace Application.Departments.Queries.GetAll;

public class GetAllDepartmentsQueryHandler(IDepartmentRepository repository) : IQueryHandler<GetAllDepartmentsQuery, GetAllDepartmentsQueryResponse>
{
    public async Task<Result<GetAllDepartmentsQueryResponse>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    {
        var departments = await repository.GetByConditionAsync(c => true, request.PaginationParameters, false, cancellationToken);

        var list = departments.Items.Select(d => new GetDepartmentByIdQueryResponse(d.Id, d.Name, d.Description)).ToList();

        var pagedList = new PagedList<GetDepartmentByIdQueryResponse>(list, departments.PageNumber, departments.PageSize, departments.TotalCount);

        var response = new GetAllDepartmentsQueryResponse(pagedList);

        return response;
    }
}
