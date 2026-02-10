using CrecheManagement.Domain.Commands.Auth;
using CrecheManagement.Domain.Messages;
using FluentValidation;

namespace CrecheManagement.Domain.Validators.Commands.Auth;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Username).NotNull().NotEmpty().WithMessage(ReturnMessages.NAME_REQUIRED);
        RuleFor(x => x.KeepAlive).NotNull().NotEmpty().WithMessage(ReturnMessages.KEEPALIVE_REQUIRED);

        RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage(ReturnMessages.EMAIL_REQUIRED);
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage(ReturnMessages.EMAIL_INVALID)
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Password)
            .NotNull().NotEmpty().WithMessage(ReturnMessages.PASSWORD_REQUIRED)
            .MinimumLength(8).MaximumLength(24).WithMessage(ReturnMessages.PASSWORD_LENGTH);
    }
}