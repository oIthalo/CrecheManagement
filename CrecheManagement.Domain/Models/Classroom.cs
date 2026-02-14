using CrecheManagement.Domain.Dtos;

namespace CrecheManagement.Domain.Models;

public class Classroom : BaseModel
{
    public string CrecheIdentifier { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
    public List<ClassroomStudentsDto> Students { get; set; }

    public Classroom()
    {
        Students = new List<ClassroomStudentsDto>();
    }
}