using MediatR;

namespace CrecheManagement.Domain.Commands.Creche;

public class DeleteCrecheCommand : IRequest
{
    public string Identifier { get; set; }
}