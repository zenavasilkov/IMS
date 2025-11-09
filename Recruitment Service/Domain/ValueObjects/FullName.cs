using Domain.Primitives;
using Domain.Shared;
using static Domain.Errors.DomainErrors;

namespace Domain.ValueObjects;

public class FullName : ValueObject
{
    public const int MaxLength = 50;

    private const string _default = "Default";

    public static readonly FullName Default = Create(_default, _default, _default).Value;

    private FullName(string firstName, string lastName, string? patronymic = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Patronymic = patronymic;
    }

    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? Patronymic { get; init; }


    public override IEnumerable<object?> GetAtomicValues()
    {
        yield return FirstName;
        yield return LastName;
        yield return Patronymic;
    }

    public static Result<FullName> Create(string firstName, string lastName, string? patronymic)
    {
        if (string.IsNullOrWhiteSpace(firstName)) return EmployeeErrors.EmptyFirstName;
        if (string.IsNullOrWhiteSpace(lastName)) return EmployeeErrors.EmptyLastName;

        if (firstName.Length > MaxLength) return EmployeeErrors.FirstNameTooLong;
        if (lastName.Length > MaxLength) return EmployeeErrors.LastNameTooLong;
        if (patronymic is not null && patronymic.Length > MaxLength) return EmployeeErrors.PatronymicTooLong;

        return new FullName(firstName, lastName, patronymic);
    }
}
