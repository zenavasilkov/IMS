using FluentValidation;
using IMS.Presentation.DTOs.CreateDTO;

namespace IMS.Presentation.Validation.Validators.TicketDtoValidators;

public class TicketDtoValidator : AbstractValidator<CreateTicketDTO>
{
    public TicketDtoValidator()
    {
        RuleFor(x => x.BoardId)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

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
