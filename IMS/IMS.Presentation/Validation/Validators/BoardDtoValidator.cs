using FluentValidation;
using IMS.Presentation.DTOs.UpdateDTO;

namespace IMS.Presentation.Validation.Validators;

public class BoardDtoValidator : AbstractValidator<UpdateBoardDTO>
{
    public BoardDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(ValidationConstants.NotEmpty);
    }
}
