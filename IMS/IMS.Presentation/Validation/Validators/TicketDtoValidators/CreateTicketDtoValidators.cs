using FluentValidation;
using IMS.Presentation.DTOs.CreateDTO;

namespace IMS.Presentation.Validation.Validators.TicketDtoValidators;

public class TicketDtoValidator : AbstractValidator<CreateTicketDto>
{
    public TicketDtoValidator()
    {
        RuleFor(x => x.BoardId)
            .NotNull()
            .WithMessage(ValidationConstants.NotNull)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.DeadLine)
            .NotNull()
            .WithMessage(ValidationConstants.NotNull)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage(ValidationConstants.DateInPast);
    }
}
