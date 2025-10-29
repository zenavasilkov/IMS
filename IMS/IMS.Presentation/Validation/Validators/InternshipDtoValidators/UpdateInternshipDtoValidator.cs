using FluentValidation;
using IMS.Presentation.DTOs.UpdateDTO;

namespace IMS.Presentation.Validation.Validators.InternshipDtoValidators;

public class UpdateInternshipDtoValidator : AbstractValidator<UpdateInternshipDTO>
{
    public UpdateInternshipDtoValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.EndDate.HasValue)
            .WithMessage(ValidationConstants.InvalidEndDate);
    }
}
