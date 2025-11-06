using Domain.Shared;
using static Domain.Errors.DomainErrors;

namespace Domain.Entities;

public sealed class Candidate : Entity
{
    private Candidate(
        Guid id,
        string firstName,
        string lastName,
        string email,
        bool isApplied,
        string? phoneNumber = null,
        string? cvLink = null,
        string? linkedIn = null,
        string? patronymic = null) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        IsApplied = isApplied;
        PhoneNumber = phoneNumber;
        CvLink = cvLink;
        LinkedIn = linkedIn;
        Patronymic = patronymic;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public bool IsApplied { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? CvLink { get; private set; }
    public string? LinkedIn { get; private set; }
    public string? Patronymic { get; private set; }

    public static Result<Candidate> Create(
        Guid id,
        string firstName,
        string lastName,
        string email,
        bool isApplied,
        string? phoneNumber = null,
        string? cvLink = null,
        string? linkedIn = null,
        string? patronymic = null)
    {
        if (id == Guid.Empty) return CandidateErrors.EmptyId;

        if (string.IsNullOrWhiteSpace(firstName)) return CandidateErrors.EmptyFirstName;

        if (string.IsNullOrWhiteSpace(lastName)) return CandidateErrors.EmptyLastName;

        if (!Validator.IsValidEmail(email)) return CandidateErrors.InvalidEmail;

        var candidate =  new Candidate(id, firstName.Trim(), lastName.Trim(), email.Trim(),
            isApplied, phoneNumber?.Trim(), cvLink?.Trim(), linkedIn?.Trim(), patronymic?.Trim());

        return candidate;
    }

    public Result Apply()
    {
        if (IsApplied) return CandidateErrors.AlreadyApplied;

        IsApplied = true;

        return Result.Success();
    }

    public Result UpdateCvLink(string? newCvLink)
    {
        CvLink = newCvLink;

        return Result.Success();
    }
}
