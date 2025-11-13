using Domain.Enums;
using Domain.Primitives;
using Domain.Shared;
using static Domain.Errors.DomainErrors.InterviewErrors;
using RecruitmentNotifications.Messages;

namespace Domain.Entities;

public sealed class Interview : Entity
{
    private Interview(Guid id) : base(id) { }

    private Interview() : base(Guid.NewGuid()) { }

    public Guid CandidateId { get; private set; }
    public Guid InterviewerId { get; private set; }
    public Guid DepartmentId { get; private set; }
    public InterviewType Type { get; private set; }
    public DateTime ScheduledAt { get; private set; }
    public string? Feedback { get; private set; }
    public bool IsPassed { get; private set; } = false;
    public bool IsCancelled { get; private set; } = false;

    public Candidate? Candidate { get; private set; }
    public Employee? Interviewer { get; private set; }
    public Department? Department { get; private set; }

    public static Result<Interview> Create(
        Guid id,
        Candidate candidate,
        Employee interviewer,
        Department department,
        InterviewType type,
        DateTime scheduledAt)
    {
        if (id == Guid.Empty) return EmptyId;

        if (candidate is null) return NullCandidate;

        if (interviewer is null) return NullInterviewer;

        if (department is null) return NullDepartment;

        if (scheduledAt < DateTime.UtcNow.Date) return ScheduledInPast;

        var interview = new Interview(id)
        {
            Candidate = candidate,
            Interviewer = interviewer,
            Department = department,
            Type = type,
            ScheduledAt = scheduledAt,
            CandidateId = candidate.Id,
            InterviewerId = interviewer.Id,
            DepartmentId = department.Id
        };

        interview.Raise(new InterviewScheduledEvent(candidate.Email, interviewer.Email, scheduledAt, type.ToString()));

        return interview;
    }

    public Result Reschedule(DateTime newDate)
    {
        if (newDate < DateTime.UtcNow.Date) return ScheduledInPast;

        ScheduledAt = newDate;
        IsCancelled = false;

        Raise(new InterviewRescheduledEvent(Candidate!.Email, Interviewer!.Email, Type.ToString(), ScheduledAt, newDate));

        return Result.Success();
    }

    public Result AddFeedback(string feedback, bool isPassed)
    {
        if (ScheduledAt > DateTime.UtcNow) return CannotAddFeedback;

        if (string.IsNullOrWhiteSpace(feedback)) return EmptyFeedback;

        Feedback = feedback.Trim();
        IsPassed = isPassed;

        return Result.Success();
    }

    public Result Cancel()
    {
        if (ScheduledAt < DateTime.UtcNow.Date) return CancelPassedInterview;

        if (IsCancelled) return AlreadyCancelled;

        IsCancelled = true;

        Raise(new InterviewCancelledEvent(Candidate!.Email, Interviewer!.Email, Type.ToString(), ScheduledAt));

        return Result.Success();
    }
}
