using FluentValidation;
using IMS.Presentation.DTOs.UpdateDTO;

namespace IMS.Presentation.Validation.Validators;

public class UserDtoValidator : AbstractValidator<UpdateUserDTO>
{
    public UserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty)
            .EmailAddress()
            .WithMessage(ValidationConstants.InvalidEmail);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty)
            .Matches(ValidationConstants.PhoneNumberFormat)
            .WithMessage("Invalid phone number format.");

        RuleFor(x => x.Firstname)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.Lastname)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage(ValidationConstants.InvalidRole);
    }
}
