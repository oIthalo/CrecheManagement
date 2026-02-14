using System.Net;
using CrecheManagement.Domain.Commands.Student;
using CrecheManagement.Domain.Dtos;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using MediatR;

namespace CrecheManagement.Domain.Handlers.Commands.Student;

public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand>
{
    private readonly IStudentsRepository _studentsRepository;
    private readonly ICrechesRepository _crechesRepository;
    private readonly IClassroomsRepository _classroomsRepository;
    private readonly ILoggedUser _loggedUser;

    public UpdateStudentCommandHandler(
        IStudentsRepository studentsRepository,
        ICrechesRepository crechesRepository,
        ILoggedUser loggedUser,
        IClassroomsRepository classroomsRepository)
    {
        _studentsRepository = studentsRepository;
        _crechesRepository = crechesRepository;
        _loggedUser = loggedUser;
        _classroomsRepository = classroomsRepository;
    }

    public async Task Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var user = await _loggedUser.GetUserAsync();
        var creche = await _crechesRepository.GetByIdentifierAsync(request.CrecheIdentifier!);

        if (creche == null || creche.UserIdentifier != user.Identifier)
            throw new CrecheManagementException(ReturnMessages.CRECHE_NOT_FOUND, HttpStatusCode.NotFound);

        var student = await _studentsRepository.GetStudentAsync(request.StudentIdentifier!)
            ?? throw new CrecheManagementException(ReturnMessages.STUDENT_NOT_FOUND, HttpStatusCode.NotFound);

        student.ContactNumber = !string.IsNullOrEmpty(request.ContactNumber) ? student.ContactNumber : student.ContactNumber;
        student.Active = request.Active ?? student.Active;

        if (!string.IsNullOrEmpty(request.ClassroomIdentifier))
        {
            if (student.ClassroomIdentifier == request.ClassroomIdentifier)
                throw new CrecheManagementException(ReturnMessages.STUDENT_ALREADY_ON_THIS_CLASSROOM, HttpStatusCode.Conflict);

            var newClassroom = await _classroomsRepository.GetByIdentifierAsync(request.ClassroomIdentifier)
                ?? throw new CrecheManagementException(ReturnMessages.CLASSROOM_NOT_FOUND, HttpStatusCode.NotFound);

            var oldClassroom = await _classroomsRepository.GetByIdentifierAsync(student.ClassroomIdentifier);

            var studentOnOldClassroom = oldClassroom!.Students.FirstOrDefault(s => s.Identifier == student.Identifier);
            if (studentOnOldClassroom != null)
                oldClassroom!.Students.Remove(studentOnOldClassroom);

            student.ClassroomIdentifier = newClassroom.Identifier;
            student.Classroom = newClassroom.Name;

            newClassroom.Students.Add(new ClassroomStudentsDto
            {
                Identifier = student.Identifier,
                Name = student.Name,
                RegistrationId = student.RegistrationId
            });

            await _classroomsRepository.UpsertAsync(newClassroom);
            await _classroomsRepository.UpsertAsync(oldClassroom);
        }

        await _studentsRepository.UpsertAsync(student);
    }
}