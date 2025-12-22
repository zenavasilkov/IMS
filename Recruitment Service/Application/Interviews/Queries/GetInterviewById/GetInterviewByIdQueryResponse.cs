using Domain.Enums;

namespace Application.Interviews.Queries.GetInterviewById;

public sealed record GetInterviewByIdQueryResponse(
    Guid Id,
    Guid CandidateId,
    Guid InterviewerId,
    Guid DepartmentId,
    string CandidateEmail,
    string InterviewerEmail,
    string DeparnmentName,
    InterviewType InterviewType,
    DateTime ScheduledAt,
    string? Feedback,
    bool IsPassed,
    bool IsCancelled);
