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
        bool isPassed) : base(id)
    {
        Candidate = candidate;
        Interviewer = interviewer;
        Department = department;
        ScheduledAt = scheduledAt;
        Feedback = feedback;
        IsPassed = isPassed;
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
        if (id == Guid.Empty)
            return Result.Failure<Interview>(InterviewErrors.EmptyId);

        if (candidate is null)
            return Result.Failure<Interview>(InterviewErrors.NullCandidate);

        if (interviewer is null)
            return Result.Failure<Interview>(InterviewErrors.NullInterviewer);

        if (department is null)
            return Result.Failure<Interview>(InterviewErrors.NullDepartment);

        if (scheduledAt < DateTime.UtcNow.Date)
            return Result.Failure<Interview>(InterviewErrors.ScheduledInPast);

        if(string.IsNullOrWhiteSpace(feedback))
            return Result.Failure<Interview>(InterviewErrors.EmptyFeedback);

        var interview = new Interview(id, candidate, interviewer, department, scheduledAt, feedback?.Trim() ?? "", isPassed);

        return interview;
    }
}
