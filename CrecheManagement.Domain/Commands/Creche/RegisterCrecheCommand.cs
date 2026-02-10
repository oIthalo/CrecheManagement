using CrecheManagement.Domain.Responses.Creche;
using CrecheManagement.Domain.Utils;
using CrecheManagement.Domain.ValueObjects;
using MediatR;

namespace CrecheManagement.Domain.Commands.Creche;

public class RegisterCrecheCommand : IRequest<BaseResponse<CrecheResponse>>
{
    public string CNPJ { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }
}