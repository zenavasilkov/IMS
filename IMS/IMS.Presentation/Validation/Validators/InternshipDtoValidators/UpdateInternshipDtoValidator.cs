using FluentValidation;
using IMS.Presentation.DTOs.UpdateDTO;

namespace IMS.Presentation.Validation.Validators.InternshipDtoValidators;

public class UpdateInternshipDtoValidator : AbstractValidator<UpdateInternshipDTO>
{
    public UpdateInternshipDtoValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty)
            .IsInEnum()
            .WithMessage(ValidationConstants.InvalidStatus);
    }
}
