using System.Net;
using CrecheManagement.Domain.Commands.Creche;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.HttpClient.CNPJ;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Models;
using CrecheManagement.Domain.Responses.Creche;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Services;

public class RegisterCrecheCommandHandler : IRequestHandler<RegisterCrecheCommand, BaseResponse<CrecheResponse>>
{
    private readonly ICrechesRepository _crecheRepository;
    private readonly ICNPJRefitClient _cnpjrefit;

    public RegisterCrecheCommandHandler(ICrechesRepository crecheRepository, ICNPJRefitClient cnpjrefit)
    {
        _crecheRepository = crecheRepository;
        _cnpjrefit = cnpjrefit;
    }

    public async Task<BaseResponse<CrecheResponse>> Handle(RegisterCrecheCommand request, CancellationToken cancellationToken)
    {
        await _cnpjrefit.GetCompany(request.CNPJ);

        if (await _crecheRepository.ExistAsync(request.CNPJ))
            throw new CrecheManagementException(ReturnMessages.ALREADY_EXIST_CRECHE_WITH_CNPJ, HttpStatusCode.Conflict);

        var creche = new Creche()
        {
            CNPJ = request.CNPJ,
            Name = request.Name,
            Email = request.Email,
            Address = request.Address,
        };

        await _crecheRepository.AddAsync(creche);

        return new BaseResponse<CrecheResponse>()
        {
            Message = ReturnMessages.CRECHE_REGISTERED_SUCCESSFULLY,
            StatusCode = HttpStatusCode.Created,
            Data = new CrecheResponse(creche.Identifier, creche.Name)
        };
    }
}