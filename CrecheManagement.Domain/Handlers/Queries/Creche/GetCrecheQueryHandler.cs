using System.Net;
using AutoMapper;
using CrecheManagement.Domain.Interfaces.Security;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Queries.Creche;
using CrecheManagement.Domain.Responses.Creche;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Handlers.Queries.Creche;

public class GetCrecheQueryHandler : IRequestHandler<GetCrecheQuery, BaseResponse<CrecheResponse>>
{
    private readonly ILoggedUser _loggedUserService;
    private readonly ICrecheAuthorizationService _crecheService;
    private readonly IMapper _mapper;

    public GetCrecheQueryHandler(ILoggedUser loggedUserService, IMapper mapper, ICrecheAuthorizationService crecheService)
    {
        _loggedUserService = loggedUserService;
        _mapper = mapper;
        _crecheService = crecheService;
    }

    public async Task<BaseResponse<CrecheResponse>> Handle(GetCrecheQuery request, CancellationToken cancellationToken)
    {
        var user = await _loggedUserService.GetUserAsync();
        var creche = await _crecheService.AuthorizeAndGetCreche(request.Identifier);

        return new BaseResponse<CrecheResponse>()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = ReturnMessages.CRECHES_RETURNED_SUCESSFULLY,
            Data = _mapper.Map<CrecheResponse>(creche)
        };
    }
}