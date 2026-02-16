using System.Net;
using CrecheManagement.Domain.Commands.Classroom;
using CrecheManagement.Domain.Dtos;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Handlers.Commands.Classroom;

public class InsertStudentsToClassroomCommandHandler : IRequestHandler<InsertStudentsToClassroomCommand, BaseResponse<List<ClassroomStudentsDto>>>
{
    private readonly IClassroomsRepository _classroomsRepository;
    private readonly IStudentsRepository _studentsRepository;
    private readonly ICrecheAuthorizationService _crecheAuthorizationService;

    public InsertStudentsToClassroomCommandHandler(
        IClassroomsRepository classroomsRepository,
        IStudentsRepository studentsRepository,
        ICrecheAuthorizationService crecheAuthorizationService)
    {
        _classroomsRepository = classroomsRepository;
        _studentsRepository = studentsRepository;
        _crecheAuthorizationService = crecheAuthorizationService;
    }

    public async Task<BaseResponse<List<ClassroomStudentsDto>>> Handle(InsertStudentsToClassroomCommand request, CancellationToken cancellationToken)
    {
        var creche = await _crecheAuthorizationService.AuthorizeAndGetCreche(request.CrecheIdentifier!);

        var classroom = await _classroomsRepository.GetByIdentifierAsync(request.ClassroomIdentifier!)
            ?? throw new CrecheManagementException(ReturnMessages.CLASSROOM_NOT_FOUND, HttpStatusCode.NotFound);

        var existStudentsOnThisClassroom = ExistStudentsOnThisClassroom(classroom, request.StudentsIdentifiers!);
        if (existStudentsOnThisClassroom.Exist)
        {
            var studentsExisting = await _studentsRepository.GetStudentsAsync(existStudentsOnThisClassroom.ExistingStudents);
            var studentsExistingMessage = string.Join(Environment.NewLine, studentsExisting.Select(x => $"{x.Name} ({x.RegistrationId})"));

            var errorMessage = studentsExisting.Count == 1
                ? $"{ReturnMessages.STUDENT_EXIST_ON_THIS_CLASSROOM} {studentsExistingMessage}."
                : $"{ReturnMessages.STUDENTS_EXIST_ON_THIS_CLASSROOM}{Environment.NewLine}{studentsExistingMessage}.";

            throw new CrecheManagementException(errorMessage, HttpStatusCode.Conflict);
        }

        var students = await _studentsRepository.GetStudentsAsync(request.StudentsIdentifiers!);
        if (students.Count == 0)
            throw new CrecheManagementException(ReturnMessages.NONE_STUDENT_FOUND, HttpStatusCode.NotFound);

        var response = new List<ClassroomStudentsDto>();
        foreach (var student in students)
        {
            student.ClassroomIdentifier = classroom.Identifier;
            student.Classroom = classroom.Name;

            response.Add(new ClassroomStudentsDto
            {
                Identifier = student.Identifier,
                Name = student.Name,
                RegistrationId = student.RegistrationId
            });
        }

        classroom.Students.AddRange(response);

        await _classroomsRepository.UpsertAsync(classroom);
        await _studentsRepository.UpsertRangeAsync(students);

        return new BaseResponse<List<ClassroomStudentsDto>>
        {
            StatusCode = HttpStatusCode.OK,
            Message = ReturnMessages.STUDENTS_INSERTEDS_ON_CLASSROOM,
            Data = response
        };
    }

    public static (bool Exist, List<string> ExistingStudents) ExistStudentsOnThisClassroom(Models.Classroom classroom, List<string> studentsIdentifiers)
    {
        var existingStudents = classroom.Students
            .Where(x => studentsIdentifiers.Contains(x.Identifier))
            .Select(x => x.Identifier)
            .ToList();

        var exist = existingStudents.Count != 0;

        return (exist, existingStudents);
    }
}