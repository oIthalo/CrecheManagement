using CrecheManagement.Domain.Commands.Student;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Utils;
using FluentValidation;

namespace CrecheManagement.Domain.Validators.Commands.Student;

public class RegisterStudentCommandValidator : AbstractValidator<RegisterStudentCommand>
{
    public RegisterStudentCommandValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage(ReturnMessages.NAME_REQUIRED);
        RuleFor(x => x.CrecheIdentifier).NotNull().NotEmpty().WithMessage(ReturnMessages.CRECHE_IDENTIFIER_REQUIRED);
        RuleFor(x => x.CPF).NotNull().NotEmpty().WithMessage(ReturnMessages.CPF_REQUIRED);
        RuleFor(x => x.BirthDate).NotNull().WithMessage(ReturnMessages.BIRTH_DATE_REQUIRED);
        RuleFor(x => x.Gender).NotNull().WithMessage(ReturnMessages.GENDER_REQUIRED);

        RuleFor(x => x.CPF)
            .Must(Util.IsCPF).WithMessage(ReturnMessages.CPF_INVALID)
            .When(x => !string.IsNullOrEmpty(x.CPF));

        RuleFor(x => x.ContactNumber)
            .Must(Util.IsPhoneNumber).WithMessage(ReturnMessages.CONTACT_NUMBER_INVALID)
            .When(x => !string.IsNullOrEmpty(x.ContactNumber));
    }
}