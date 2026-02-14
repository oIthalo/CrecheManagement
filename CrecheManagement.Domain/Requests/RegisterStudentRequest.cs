using CrecheManagement.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace CrecheManagement.Domain.Requests;

public class RegisterStudentRequest
{
    public string Name { get; set; }
    public string CPF { get; set; }
    public string? ContactNumber { get; set; }
    public int Age { get; set; }
    public DateTime BirthDate { get; set; }
    public EGender Gender { get; set; }
    public List<IFormFile> Documents { get; set; }
}