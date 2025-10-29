using FluentValidation;
using IMS.Presentation.DTOs.UpdateDTO;

namespace IMS.Presentation.Validation.Validators.FeedbackDtoValidators;

public class UpdateFeedbackDtoValidator : AbstractValidator<UpdateFeedbackDto>
{
    public UpdateFeedbackDtoValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);
    }
}
