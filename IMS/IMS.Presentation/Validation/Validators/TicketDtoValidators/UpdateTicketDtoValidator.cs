using FluentValidation;
using IMS.Presentation.DTOs.UpdateDTO;

namespace IMS.Presentation.Validation.Validators.TicketDtoValidators;

public class UpdateTicketDtoValidator : AbstractValidator<UpdateTicketDto>
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
            .NotNull()
            .WithMessage(ValidationConstants.NotNull)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);
    }
}
