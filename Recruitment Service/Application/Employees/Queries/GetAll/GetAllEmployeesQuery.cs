using Application.Abstractions.Messaging;
using Pagination;

namespace Application.Employees.Queries.GetAll;

public sealed record GetAllEmployeesQuery(PaginationParameters PaginationParameters) : IQuery<GetAllEmployeesQueryResponse>;
