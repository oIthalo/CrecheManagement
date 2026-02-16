using System.Net;
using AutoMapper;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Security;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Queries.Creche;
using CrecheManagement.Domain.Responses.Creche;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Handlers.Queries.Creche;

public class GetCrechesQueryHandler : IRequestHandler<GetCrechesQuery, BaseResponse<List<CrecheResponse>>>
{
    private readonly ICrechesRepository _crechesRepository;
    private readonly ILoggedUser _loggedUserService;
    private readonly IMapper _mapper;

    public GetCrechesQueryHandler(ICrechesRepository crechesRepository, ILoggedUser loggedUserService, IMapper mapper)
    {
        _crechesRepository = crechesRepository;
        _loggedUserService = loggedUserService;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<CrecheResponse>>> Handle(GetCrechesQuery request, CancellationToken cancellationToken)
    {
        var user = await _loggedUserService.GetUserAsync();
        var creches = await _crechesRepository.GetAllAsync(user.Identifier);

        return new BaseResponse<List<CrecheResponse>>()
        {
            StatusCode = HttpStatusCode.OK,
            Message = ReturnMessages.CRECHES_RETURNED_SUCESSFULLY,
            Data = _mapper.Map<List<CrecheResponse>>(creches)
        };
    }
}