using FluentValidation;
using OnZeroId.Application.Features.Users.Commands.RegisterUser;

namespace OnZeroId.Application.Validation;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty().EmailAddress();
        RuleFor(x => x.Request.Password)
            .NotEmpty().MinimumLength(6);
    }
}
