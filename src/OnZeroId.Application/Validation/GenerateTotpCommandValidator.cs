using FluentValidation;
using OnZeroId.Application.Features.Users.Commands.GenerateTotp;

namespace OnZeroId.Application.Validation;

public class GenerateTotpCommandValidator : AbstractValidator<GenerateTotpCommand>
{
    public GenerateTotpCommandValidator()
    {
        RuleFor(x => x.Request.UserId).NotEmpty();
    }
}
