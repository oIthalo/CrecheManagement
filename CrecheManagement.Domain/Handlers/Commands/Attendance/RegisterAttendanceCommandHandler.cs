using System.Net;
using CrecheManagement.Domain.Commands.Attendance;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Handlers.Commands.Attendance;

public class RegisterAttendanceCommandHandler : IRequestHandler<RegisterAttendanceCommand, BaseResponse>
{
    private readonly ICrechesRepository _crechesRepository;
    private readonly IClassroomsRepository _classroomsRepository;
    private readonly IStudentsRepository _studentsRepository;
    private readonly IAttendancesRepository _attendancesRepository;
    private readonly ILoggedUser _loggedUser;

    public RegisterAttendanceCommandHandler(
        ICrechesRepository crechesRepository, 
        IClassroomsRepository classroomsRepository, 
        IStudentsRepository studentsRepository, 
        IAttendancesRepository attendancesRepository,
        ILoggedUser loggedUser)
    {
        _crechesRepository = crechesRepository;
        _classroomsRepository = classroomsRepository;
        _studentsRepository = studentsRepository;
        _attendancesRepository = attendancesRepository;
        _loggedUser = loggedUser;
    }

    public async Task<BaseResponse> Handle(RegisterAttendanceCommand request, CancellationToken cancellationToken)
    {
        var user = await _loggedUser.GetUserAsync();
        var creche = await _crechesRepository.GetByIdentifierAsync(request.CrecheIdentifier!);

        if (creche == null || creche.UserIdentifier != user.Identifier)
            throw new CrecheManagementException(ReturnMessages.CRECHE_NOT_FOUND, HttpStatusCode.NotFound);

        var classroom = await _classroomsRepository.GetByIdentifierAsync(request.ClassroomIdentifier!)
            ?? throw new CrecheManagementException(ReturnMessages.CLASSROOM_NOT_FOUND, HttpStatusCode.NotFound);

        var studentsIdentifier = classroom.Students
            .Select(x => x.Identifier)
            .ToList();

        var students = await _studentsRepository.GetStudentsAsync(studentsIdentifier);
        var studentsDict = students.ToDictionary(s => s.Identifier, s => s);

        var attendancesRegisteredToday = await _attendancesRepository.GetStudentsWithAttendanceRegisteredToday(classroom.Identifier);
        var attendancesToRegister = new List<Models.Attendance>();

        foreach (var attendance in request.Attendances)
        {
            if (!studentsDict.TryGetValue(attendance.StudentIdentifier, out var student))
                throw new CrecheManagementException(ReturnMessages.SOME_STUDENT_NOT_FOUND, HttpStatusCode.NotFound);

            if (attendancesRegisteredToday.Contains(attendance.StudentIdentifier))
                throw new CrecheManagementException($"{ReturnMessages.ATTENDANCE_ALREADY_REGISTERED_TO} {student.Name}", HttpStatusCode.Conflict);

            attendancesToRegister.Add(new Models.Attendance
            {
                StudentIdentifier = student.Identifier,
                ClassroomIdentifier = classroom.Identifier,
                CrecheIdentifier = creche.Identifier,
                Justification = attendance.Justification,
                RegisteredBy = request.RegisteredBy,
                StudentName = student.Name,
                Date = request.Date.Date,
                Status = attendance.Status,
            });
        }

        await _attendancesRepository.InsertRangeAsync(attendancesToRegister);

        return new BaseResponse
        {
            StatusCode = HttpStatusCode.OK,
            Message = ReturnMessages.ATTENDANCES_REGISTERED_SUCCESSFULLY
        };
    }
}