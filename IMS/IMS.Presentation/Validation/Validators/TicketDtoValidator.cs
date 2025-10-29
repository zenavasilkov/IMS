using FluentValidation;
using IMS.Presentation.DTOs.UpdateDTO;

namespace IMS.Presentation.Validation.Validators;

public class TicketDtoValidator : AbstractValidator<UpdateTicketDTO>
{
    public TicketDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty)
            .IsInEnum()
            .WithMessage(ValidationConstants.InvalidStatus);

        RuleFor(x => x.DeadLine)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage(ValidationConstants.DateInPast);

    }
}
