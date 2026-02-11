using CrecheManagement.Domain.ValueObjects;

namespace CrecheManagement.Domain.Models;

public class Creche : BaseModel
{
    public string UserIdentifier { get; set; }
    public string CNPJ { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string ContactNumber { get; set; }
    public Address Address { get; set; }
}