using System.Text.Json.Serialization;
using CrecheManagement.Domain.Enums;
using CrecheManagement.Domain.Responses.Student;
using CrecheManagement.Domain.Utils;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CrecheManagement.Domain.Commands.Student;

public class RegisterStudentCommand : IRequest<BaseResponse<RegisteredStudentResponse>>
{
    [JsonIgnore]
    public string? CrecheIdentifier { get; set; }
    public string Name { get; set; }
    public string CPF { get; set; }
    public string? ContactNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public EGender Gender { get; set; }
    public List<IFormFile> Documents { get; set; }
}