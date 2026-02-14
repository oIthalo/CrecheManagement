using System.Net;
using CrecheManagement.Domain.Commands.Classroom;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using MediatR;

namespace CrecheManagement.Domain.Handlers.Commands.Classroom;

public class DeleteClassroomCommandHandler : IRequestHandler<DeleteClassroomCommand>
{
    private readonly ICrechesRepository _crechesRepository;
    private readonly IClassroomsRepository _classroomsRepository;
    private readonly IStudentsRepository _studentsRepository;
    private readonly ILoggedUser _loggedUser;

    public DeleteClassroomCommandHandler(
        ICrechesRepository crechesRepository,
        IClassroomsRepository classroomsRepository, 
        IStudentsRepository studentsRepository, 
        ILoggedUser loggedUser)
    {
        _crechesRepository = crechesRepository;
        _classroomsRepository = classroomsRepository;
        _studentsRepository = studentsRepository;
        _loggedUser = loggedUser;
    }

    public async Task Handle(DeleteClassroomCommand request, CancellationToken cancellationToken)
    {
        var user = await _loggedUser.GetUserAsync();
        var creche = await _crechesRepository.GetByIdentifierAsync(request.CrecheIdentifier!);

        if (creche == null || creche.UserIdentifier != user.Identifier)
            throw new CrecheManagementException(ReturnMessages.CRECHE_NOT_FOUND, HttpStatusCode.NotFound);

        var classroom = await _classroomsRepository.GetByIdentifierAsync(request.ClassroomIdentifier)
             ?? throw new CrecheManagementException(ReturnMessages.CLASSROOM_NOT_FOUND, HttpStatusCode.NotFound);

        var studentsIdentifier = classroom.Students
            .Select(x => x.Identifier)
            .ToList();

        var students = await _studentsRepository.GetStudentsAsync(studentsIdentifier);
        if (students.Count > 0)
        {
            foreach (var student in students)
            {
                student.ClassroomIdentifier = null;
                student.Classroom = null;
            }

            await _studentsRepository.UpsertRangeAsync(students);
        }

        await _classroomsRepository.DeleteAsync(classroom);
    }
}