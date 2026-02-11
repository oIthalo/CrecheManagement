using CrecheManagement.Domain.Commands.Creche;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Utils;
using CrecheManagement.Domain.Validators.ValueObjects;
using FluentValidation;

namespace CrecheManagement.Domain.Validators.Commands.Creche;

public class UpdateCrecheCommandValidator : AbstractValidator<UpdateCrecheCommand>
{
    public UpdateCrecheCommandValidator()
    {
        RuleFor(x => x.Email)
           .EmailAddress().WithMessage(ReturnMessages.EMAIL_INVALID)
           .When(x => x.Email != null);

        RuleFor(x => x.ContactNumber)
            .Must(Util.IsPhoneNumber).WithMessage(ReturnMessages.CONTACT_NUMBER_INVALID)
            .When(x => x.ContactNumber != null);

        RuleFor(x => x.Address)
            .SetValidator(new AddressValidator())
            .When(x => x.Address != null);
    }
}