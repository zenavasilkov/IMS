using FluentValidation;
using IMS.Presentation.DTOs.CreateDTO;

namespace IMS.Presentation.Validation.Validators.InternshipDtoValidators;

public class CreateInternshipDtoValidator : AbstractValidator<CreateInternshipDTO>
{
    public CreateInternshipDtoValidator()
    {
        RuleFor(x => x.InternId)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.MentorId)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.HumanResourcesManagerId)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.EndDate.HasValue)
            .WithMessage(ValidationConstants.InvalidEndDate);
    }
}
