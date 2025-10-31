using FluentValidation;
using IMS.Presentation.DTOs.CreateDTO;

namespace IMS.Presentation.Validation.Validators.FeedbackDtoValidators;

public class CreateFeedbackDtoValidator : AbstractValidator<CreateFeedbackDTO>
{
    public CreateFeedbackDtoValidator()
    {
        RuleFor(x => x.TicketId)
            .NotNull()
            .WithMessage(ValidationConstants.NotNull)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.AddressedToId)
            .NotNull()
            .WithMessage(ValidationConstants.NotNull)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.SentById)
            .NotNull()
            .WithMessage(ValidationConstants.NotNull)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);
    }
}
