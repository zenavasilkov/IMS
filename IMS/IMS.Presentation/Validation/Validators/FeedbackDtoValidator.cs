using FluentValidation;
using IMS.Presentation.DTOs.UpdateDTO;

namespace IMS.Presentation.Validation.Validators;

public class FeedbackDtoValidator : AbstractValidator<UpdateFeedbackDTO>
{
    public FeedbackDtoValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);
    }
}
