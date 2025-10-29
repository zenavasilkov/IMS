namespace IMS.Presentation.Validation;

public static class ValidationConstants
{
    public const string NotEmpty = "{PropertyName} is required";

    public const string InvalidEmail = "Invalid email format";

    public const string InvalidRole = "Invalid role specified.";

    public const string InvalidStatus = "Invalid status specified";

    public const string PhoneNumberFormat = @"^\+?[1-9]\d{1,14}$";

    public const string DateInPast = "The provided date is in the past. It must be a future date";
}
