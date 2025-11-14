using Application.Abstractions.Messaging;

namespace Application.Departments.Queries.GetDepartmentById;

public sealed record GetDepartmentByIdQuery(Guid Id) : IQuery<GetDepartmentByIdQueryResponse>;
