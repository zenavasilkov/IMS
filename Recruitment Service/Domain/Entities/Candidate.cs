using Domain.Primitives;
using Domain.Shared;
using Domain.ValueObjects;
using static Domain.Errors.DomainErrors.CandidateErrors;
using RecruitmentNotifications.Messages;

namespace Domain.Entities;

public sealed class Candidate : Entity
{
    private Candidate(Guid id) : base(id) { }

    public FullName FullName { get; private set; } = FullName.Default;
    public string Email { get; private set; } = string.Empty;
    public bool IsAcceptedToInternship { get; private set; } = false;
    public string? PhoneNumber { get; private set; }
    public string? CvLink { get; private set; }
    public string? LinkedIn { get; private set; } 

    public static Result<Candidate> Create(
        Guid id, 
        FullName fullName,
        string email,
        string? phoneNumber = null,
        string? cvLink = null,
        string? linkedIn = null)
    {
        if (id == Guid.Empty) return EmptyId; 

        if (!Validator.IsValidEmail(email)) return InvalidEmail;

        var candidate = new Candidate(id)
        { 
            Email = email.Trim(),
            FullName = fullName,
            PhoneNumber = phoneNumber?.Trim(),
            CvLink = cvLink?.Trim(),
            LinkedIn = linkedIn?.Trim(), 
        };

        return candidate;
    }

    public Result AcceptCandidateToInternship()
    {
        if (IsAcceptedToInternship) return AlreadyApplied;

        IsAcceptedToInternship = true;

        Raise(new CandidatePassedToInternshipEvent(Email));

        return Result.Success();
    }

    public Result UpdateCvLink(string? newCvLink)
    {
        CvLink = newCvLink;

        return Result.Success();
    }
}
