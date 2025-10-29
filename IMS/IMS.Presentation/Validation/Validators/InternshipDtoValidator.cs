using FluentValidation;
using IMS.Presentation.DTOs.UpdateDTO;

namespace IMS.Presentation.Validation.Validators;

public class InternshipDtoValidator : AbstractValidator<UpdateInternshipDTO>
{
    public InternshipDtoValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty)
            .IsInEnum()
            .WithMessage(ValidationConstants.InvalidStatus);
    }
}
