using Application.Abstractions.Messaging;

namespace Application.Departments.Queries.GetDepartmentByName;

public sealed record GetDepartmentByNameQuery(string Name) : IQuery<GetDepartmentByNameResponse>;
