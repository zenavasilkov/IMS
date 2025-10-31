using FluentValidation;
using IMS.Presentation.DTOs.UpdateDTO;

namespace IMS.Presentation.Validation.Validators.InternshipDtoValidators;

public class UpdateInternshipDtoValidator : AbstractValidator<UpdateInternshipDto>
{
    public UpdateInternshipDtoValidator()
    {
        RuleFor(x => x.StartDate)
            .NotNull()
            .WithMessage(ValidationConstants.NotNull)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.EndDate)
            .NotNull()
            .WithMessage(ValidationConstants.NotNull)
            .GreaterThan(x => x.StartDate)
            .When(x => x.EndDate.HasValue)
            .WithMessage(ValidationConstants.InvalidEndDate);
    }
}
