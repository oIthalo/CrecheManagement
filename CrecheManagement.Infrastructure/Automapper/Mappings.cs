using AutoMapper;
using CrecheManagement.Domain.Commands.Student;
using CrecheManagement.Domain.Models;
using CrecheManagement.Domain.Requests;
using CrecheManagement.Domain.Responses.Attendance;
using CrecheManagement.Domain.Responses.Classroom;
using CrecheManagement.Domain.Responses.Creche;
using CrecheManagement.Domain.Responses.Student;

namespace CrecheManagement.Infrastructure.Mappings;

public class Mappings : Profile
{
    public Mappings()
    {
        ModelToResponse();
        RequestToCommand();
    }

    private void ModelToResponse()
    {
        CreateMap<Creche, CrecheResponse>();
        CreateMap<Classroom, ClassroomResponse>();
        CreateMap<Attendance, AttendanceResponse>();
        CreateMap<Student, StudentResponse>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()));
    }

    private void RequestToCommand()
    {
        CreateMap<RegisterStudentRequest, RegisterStudentCommand>();
    }
}