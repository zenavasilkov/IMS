using Domain.Shared;
using static Domain.Errors.DomainErrors;

namespace Domain.Entities;

public sealed class Interview : Entity
{
    private Interview(
        Guid id,
        Candidate candidate,
        Employee interviewer,
        Department department,
        DateTime scheduledAt,
        string feedback,
        bool isPassed,
        bool isCancelled) : base(id)
    {
        Candidate = candidate;
        Interviewer = interviewer;
        Department = department;
        ScheduledAt = scheduledAt;
        Feedback = feedback;
        IsPassed = isPassed;
        IsCancelled = isCancelled;
        CandidateId = candidate.Id;
        InterviewerId = interviewer.Id;
        DepartmentId = department.Id;
    }

    public Guid CandidateId { get; private set; }
    public Guid InterviewerId { get; private set; }
    public Guid DepartmentId { get; private set; }
    public DateTime ScheduledAt { get; private set; }
    public string Feedback { get; private set; }
    public bool IsPassed { get; private set; }
    public bool IsCancelled { get; private set; }

    public Candidate Candidate { get; private set; }
    public Employee Interviewer { get; private set; }
    public Department Department { get; private set; }

    public static Result<Interview> Create(
        Guid id,
        Candidate candidate,
        Employee interviewer,
        Department department,
        DateTime scheduledAt,
        string feedback,
        bool isPassed)
    {
        if (id == Guid.Empty) return InterviewErrors.EmptyId;

        if (candidate is null) return InterviewErrors.NullCandidate;

        if (interviewer is null) return InterviewErrors.NullInterviewer;

        if (department is null) return InterviewErrors.NullDepartment;

        if (scheduledAt < DateTime.UtcNow.Date) return InterviewErrors.ScheduledInPast;

        if(string.IsNullOrWhiteSpace(feedback)) return InterviewErrors.EmptyFeedback;

        var interview = new Interview(id, candidate, interviewer, department, scheduledAt, feedback.Trim(), isPassed, false);

        return interview;
    }

    public Result Reschedule(DateTime newDate)
    {
        if (newDate < DateTime.UtcNow.Date) return InterviewErrors.ScheduledInPast;

        ScheduledAt = newDate;

        return Result.Success();
    }

    public Result UpdateFeedback(string feedback, bool isPassed)
    {
        if (string.IsNullOrWhiteSpace(feedback)) return InterviewErrors.EmptyFeedback;

        Feedback = feedback.Trim();
        IsPassed = isPassed;

        return Result.Success();
    }

    public Result Cancel()
    {
        if (ScheduledAt < DateTime.UtcNow.Date) return InterviewErrors.CancelPassedInterview;

        if(IsCancelled) return InterviewErrors.AlreadyCancelled;

        IsCancelled = true;

        return Result.Success();
    }
}
