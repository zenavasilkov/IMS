using Application.Abstractions.Messaging;
using Pagination;

namespace Application.Departments.Queries.GetAll;

public sealed record GetAllDepartmentsQuery(PaginationParameters PaginationParameters) : IQuery<GetAllDepartmentsQueryResponse>;
