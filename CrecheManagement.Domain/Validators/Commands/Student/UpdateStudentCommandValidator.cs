using CrecheManagement.Domain.Commands.Student;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Utils;
using FluentValidation;

namespace CrecheManagement.Domain.Validators.Commands.Student;

public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentCommandValidator()
    {
        RuleFor(x => x.CrecheIdentifier).NotNull().NotEmpty().WithMessage(ReturnMessages.CRECHE_IDENTIFIER_REQUIRED);

        RuleFor(x => x.ContactNumber)
            .Must(Util.IsPhoneNumber).WithMessage(ReturnMessages.CONTACT_NUMBER_INVALID)
            .When(x => !string.IsNullOrEmpty(x.ContactNumber));
    }
}