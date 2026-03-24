using System.Net;
using CrecheManagement.Domain.Enums;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Queries.Dashboard;
using CrecheManagement.Domain.Responses.Dashboard;
using CrecheManagement.Domain.Utils;
using MediatR;
using MongoDB.Driver;
using static CrecheManagement.Domain.Responses.Dashboard.DashboardResponse;

namespace CrecheManagement.Domain.Handlers.Queries.Dashboard;

public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, BaseResponse<DashboardResponse>>
{
    private readonly IAttendancesRepository _attendancesRepository;
    private readonly IClassroomsRepository _classroomsRepository;
    private readonly ICrecheAuthorizationService _crecheAuthorizationService;
    private readonly IStudentsRepository _studentsRepository;

    public GetDashboardQueryHandler(
        IAttendancesRepository attendancesRepository,
        IClassroomsRepository classroomsRepository,
        ICrecheAuthorizationService crecheAuthorizationService,
        IStudentsRepository studentsRepository)
    {
        _attendancesRepository = attendancesRepository;
        _classroomsRepository = classroomsRepository;
        _crecheAuthorizationService = crecheAuthorizationService;
        _studentsRepository = studentsRepository;
    }

    public async Task<BaseResponse<DashboardResponse>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        var creche = await _crecheAuthorizationService.AuthorizeAndGetCreche(request.CrecheIdentifier!);
        var classrooms = await _classroomsRepository.GetClassroomsAsync(creche.Identifier, DateTime.Now.Year);
        var attendances = await _attendancesRepository.GetAllAttendancesTodayAsync(creche.Identifier);
        var totalStudents = await _studentsRepository.GetTotalStudents(creche.Identifier);

        var totalAbsent = attendances.Count(x => x.Status == EAttendanceStatus.Absent);
        var totalPresent = attendances.Count(x => x.Status == EAttendanceStatus.Present);
        var attendanceRate = totalStudents > 0
            ? (totalPresent * 100.0 / totalStudents)
            : 0;

        var response = new DashboardResponse
        {
            CrecheName = creche.Name,
            TotalClassrooms = classrooms.Count,
            TotalStudents = totalStudents,
            AttendanceRate = attendanceRate,
            AbsentToday = totalAbsent,
            PresentToday = totalPresent,
            Classrooms = classrooms
                .Select(x => new ClassroomDashboard
                {
                    Name = x.Name,
                    TotalStudents = x.Students.Count,
                    Absent = attendances.Where(x => x.ClassroomIdentifier == x.Identifier).Count(x => x.Status == EAttendanceStatus.Absent),
                    Present = attendances.Where(x => x.ClassroomIdentifier == x.Identifier).Count(x => x.Status == EAttendanceStatus.Present),
                }).ToList()
        };

        return new BaseResponse<DashboardResponse>
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = ReturnMessages.DASHBOARD_RETURNED_SUCCESSFULLY,
            Data = response
        };
    }
}