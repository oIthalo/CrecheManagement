using CrecheManagement.Domain.Responses.Creche;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Queries.Creche;

public class GetCrecheQuery : IRequest<BaseResponse<CrecheResponse>>
{
    public string Identifier { get; set; }
}