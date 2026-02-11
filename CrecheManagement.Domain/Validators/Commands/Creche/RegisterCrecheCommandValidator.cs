using CrecheManagement.Domain.Commands.Creche;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Utils;
using CrecheManagement.Domain.Validators.ValueObjects;
using FluentValidation;

namespace CrecheManagement.Domain.Validators.Commands.Creche;

public class RegisterCrecheCommandValidator : AbstractValidator<RegisterCrecheCommand>
{
    public RegisterCrecheCommandValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage(ReturnMessages.NAME_REQUIRED);

        RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage(ReturnMessages.EMAIL_REQUIRED);
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage(ReturnMessages.EMAIL_INVALID)
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.CNPJ).NotNull().NotEmpty().WithMessage(ReturnMessages.CNPJ_REQUIRED);
        RuleFor(x => x.CNPJ)
            .Must(Util.IsCNPJ).WithMessage(ReturnMessages.CNPJ_INVALID)
            .When(x => !string.IsNullOrEmpty(x.CNPJ));

        RuleFor(x => x.ContactNumber)
            .Must(Util.IsPhoneNumber).WithMessage(ReturnMessages.CONTACT_NUMBER_INVALID)
            .When(x => !string.IsNullOrEmpty(x.ContactNumber));

        RuleFor(x => x.Address)
            .SetValidator(new AddressValidator())
            .When(x => x.Address != null);
    }
}