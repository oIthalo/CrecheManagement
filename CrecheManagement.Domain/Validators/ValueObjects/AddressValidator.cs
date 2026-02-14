using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Utils;
using CrecheManagement.Domain.ValueObjects;
using FluentValidation;

namespace CrecheManagement.Domain.Validators.ValueObjects;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.ZipCode).NotNull().NotEmpty().WithMessage(ReturnMessages.ADDRESS_ZIP_REQUIRED);
        RuleFor(x => x.ZipCode)
            .Must(Util.IsCEP).WithMessage(ReturnMessages.ADDRESS_ZIP_INVALID);

        RuleFor(x => x.Number).NotNull().NotEmpty().WithMessage(ReturnMessages.ADDRESS_NUMBER_REQUIRED);
        RuleFor(x => x.City).NotNull().NotEmpty().WithMessage(ReturnMessages.ADDRESS_NUMBER_REQUIRED);
        RuleFor(x => x.District).NotNull().NotEmpty().WithMessage(ReturnMessages.ADDRESS_DISTRICT_REQUIRED);
        RuleFor(x => x.Street).NotNull().NotEmpty().WithMessage(ReturnMessages.ADDRESS_STREET_REQUIRED);
        RuleFor(x => x.State).NotNull().NotEmpty().WithMessage(ReturnMessages.ADDRESS_STATE_REQUIRED);
    }
}