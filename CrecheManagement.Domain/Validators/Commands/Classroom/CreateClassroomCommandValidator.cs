using CrecheManagement.Domain.Commands.Class;
using CrecheManagement.Domain.Messages;
using FluentValidation;

namespace CrecheManagement.Domain.Validators.Commands.Classroom;

public class CreateClassroomCommandValidator : AbstractValidator<CreateClassroomCommand>
{
    public CreateClassroomCommandValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage(ReturnMessages.NAME_REQUIRED);
        RuleFor(x => x.CrecheIdentifier).NotNull().NotEmpty().WithMessage(ReturnMessages.CRECHE_IDENTIFIER_REQUIRED);
    }
}