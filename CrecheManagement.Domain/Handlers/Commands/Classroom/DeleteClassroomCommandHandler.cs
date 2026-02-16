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
    private readonly IClassroomsRepository _classroomsRepository;
    private readonly IStudentsRepository _studentsRepository;
    private readonly ICrecheAuthorizationService _crecheAuthorizationService;

    public DeleteClassroomCommandHandler(
        IClassroomsRepository classroomsRepository,
        IStudentsRepository studentsRepository,
        ICrecheAuthorizationService crecheAuthorizationService)
    {
        _classroomsRepository = classroomsRepository;
        _studentsRepository = studentsRepository;
        _crecheAuthorizationService = crecheAuthorizationService;
    }

    public async Task Handle(DeleteClassroomCommand request, CancellationToken cancellationToken)
    {
        var creche = await _crecheAuthorizationService.AuthorizeAndGetCreche(request.CrecheIdentifier!);

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