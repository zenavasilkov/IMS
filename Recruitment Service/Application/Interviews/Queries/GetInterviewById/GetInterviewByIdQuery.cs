using Application.Abstractions.Messaging;

namespace Application.Interviews.Queries.GetInterviewById;

public sealed record GetInterviewByIdQuery(Guid Id) : IQuery<GetInterviewByIdQueryResponse>;
