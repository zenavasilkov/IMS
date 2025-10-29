using FluentValidation;
using IMS.Presentation.DTOs.CreateDTO;

namespace IMS.Presentation.Validation.Validators.UserDtoValidators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty)
            .Matches(ValidationConstants.PhoneNumberFormat)
            .WithMessage("Invalid phone number format.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty)
            .EmailAddress()
            .WithMessage(ValidationConstants.InvalidEmail);

        RuleFor(x => x.Role)
            .NotNull()
            .WithMessage(ValidationConstants.NotNull)
            .IsInEnum()
            .WithMessage(ValidationConstants.InvalidRole);

        RuleFor(x => x.Firstname)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.Lastname)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);
    }
}
