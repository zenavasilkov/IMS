using Application.Abstractions.Messaging;
using Domain.Enums;

namespace Application.Interviews.Commands.ScheduleInterview;

public sealed record ScheduleInterviewCommand(
    Guid CandidateId,
    Guid InterviewerId,
    Guid DepartmentId,
    InterviewType Type,
    DateTime ScheduledAt) : ICommand;
