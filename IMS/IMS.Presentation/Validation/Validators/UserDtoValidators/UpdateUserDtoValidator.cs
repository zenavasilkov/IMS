using FluentValidation;
using IMS.Presentation.DTOs.UpdateDTO;

namespace IMS.Presentation.Validation.Validators.UserDtoValidators;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .WithMessage(ValidationConstants.NotNull)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty)
            .EmailAddress()
            .WithMessage(ValidationConstants.InvalidEmail);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty)
            .Matches(ValidationConstants.PhoneNumberFormat)
            .WithMessage("Invalid phone number format.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.Role)
            .NotNull()
            .WithMessage(ValidationConstants.NotNull)
            .IsInEnum()
            .WithMessage(ValidationConstants.InvalidRole);
    }
}
