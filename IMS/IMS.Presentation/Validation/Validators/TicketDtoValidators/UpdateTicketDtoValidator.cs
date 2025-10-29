using FluentValidation;
using IMS.Presentation.DTOs.UpdateDTO;

namespace IMS.Presentation.Validation.Validators.TicketDtoValidators;

public class UpdateTicketDtoValidator : AbstractValidator<UpdateTicketDTO>
{
    public UpdateTicketDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.DeadLine)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage(ValidationConstants.DateInPast);
    }
}
