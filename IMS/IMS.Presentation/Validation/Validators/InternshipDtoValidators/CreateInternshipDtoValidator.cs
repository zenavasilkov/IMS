using FluentValidation;
using IMS.Presentation.DTOs.CreateDTO;

namespace IMS.Presentation.Validation.Validators.InternshipDtoValidators;

public class CreateInternshipDtoValidator : AbstractValidator<CreateInternshipDto>
{
    public CreateInternshipDtoValidator()
    {
        RuleFor(x => x.InternId)
            .NotNull()
            .WithMessage(ValidationConstants.NotNull)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.MentorId)
            .NotNull()
            .WithMessage(ValidationConstants.NotNull)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.HumanResourcesManagerId)
            .NotNull()
            .WithMessage(ValidationConstants.NotNull)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

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
