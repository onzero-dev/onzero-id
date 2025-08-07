using FluentValidation;
using OnZeroId.Application.Features.Users.Commands.ValidateTotp;

namespace OnZeroId.Application.Validation;

public class ValidateTotpCommandValidator : AbstractValidator<ValidateTotpCommand>
{
    public ValidateTotpCommandValidator()
    {
        RuleFor(x => x.Request.UserId).NotEmpty();
        RuleFor(x => x.Request.Code).NotEmpty().Length(6);
    }
}
