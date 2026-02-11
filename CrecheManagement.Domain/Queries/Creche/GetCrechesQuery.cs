using CrecheManagement.Domain.Responses.Creche;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Queries.Creche;

public class GetCrechesQuery : IRequest<BaseResponse<List<CrecheResponse>>>
{
}