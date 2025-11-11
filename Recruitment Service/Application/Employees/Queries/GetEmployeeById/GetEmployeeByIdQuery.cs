using Application.Abstractions.Messaging;

namespace Application.Employees.Queries.GetEmployeeById;

public sealed record GetEmployeeByIdQuery(Guid Id) : IQuery<GetEmployeeByIdQueryResponse>;
