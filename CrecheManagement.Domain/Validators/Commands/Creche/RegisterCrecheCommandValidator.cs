using CrecheManagement.Domain.Commands.Creche;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Utils;
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

        RuleFor(x => x.Address.Number).NotNull().NotEmpty().WithMessage(ReturnMessages.ADDRESS_NUMBER_REQUIRED);
        RuleFor(x => x.Address.City).NotNull().NotEmpty().WithMessage(ReturnMessages.ADDRESS_NUMBER_REQUIRED);
        RuleFor(x => x.Address.District).NotNull().NotEmpty().WithMessage(ReturnMessages.ADDRESS_DISTRICT_REQUIRED);
        RuleFor(x => x.Address.Country).NotNull().NotEmpty().WithMessage(ReturnMessages.ADDRESS_COUNTRY_REQUIRED);
        RuleFor(x => x.Address.ZipCode).NotNull().NotEmpty().WithMessage(ReturnMessages.ADDRESS_ZIP_REQUIRED);
        RuleFor(x => x.Address.Street).NotNull().NotEmpty().WithMessage(ReturnMessages.ADDRESS_STREET_REQUIRED);
        RuleFor(x => x.Address.State).NotNull().NotEmpty().WithMessage(ReturnMessages.ADDRESS_STATE_REQUIRED);
    }
}