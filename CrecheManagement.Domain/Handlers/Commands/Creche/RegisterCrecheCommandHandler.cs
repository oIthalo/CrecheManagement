using System.Net;
using CrecheManagement.Domain.Commands.Creche;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.HttpClient.CNPJ;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Models;
using CrecheManagement.Domain.Responses.Creche;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Services;

public class RegisterCrecheCommandHandler : IRequestHandler<RegisterCrecheCommand, BaseResponse<CreatedCrecheResponse>>
{
    private readonly ICrechesRepository _crecheRepository;
    private readonly ICNPJRefitClient _cnpjrefit;
    private readonly ILoggedUser _loggedUser;

    public RegisterCrecheCommandHandler(
        ICrechesRepository crecheRepository, 
        ICNPJRefitClient cnpjrefit, 
        ILoggedUser loggedUser)
    {
        _crecheRepository = crecheRepository;
        _cnpjrefit = cnpjrefit;
        _loggedUser = loggedUser;
    }

    public async Task<BaseResponse<CreatedCrecheResponse>> Handle(RegisterCrecheCommand request, CancellationToken cancellationToken)
    {
        var user = await _loggedUser.GetUserAsync();

        var cnpj = Util.KeepLettersAndNumbers(request.CNPJ);
        var contactNumber = Util.KeepLettersAndNumbers(request.ContactNumber);

        await _cnpjrefit.GetCompany(cnpj);

        if (await _crecheRepository.ExistWithCNPJAsync(cnpj))
            throw new CrecheManagementException(ReturnMessages.ALREADY_EXIST_CRECHE_WITH_CNPJ, HttpStatusCode.Conflict);

        if (await _crecheRepository.ExistWithEmailAsync(request.Email))
            throw new CrecheManagementException(ReturnMessages.ALREADY_EXIST_CRECHE_WITH_EMAIL, HttpStatusCode.Conflict);

        var creche = new Creche()
        {
            UserIdentifier = user.Identifier,
            ContactNumber = contactNumber,
            CNPJ = cnpj,
            Name = request.Name,
            Email = request.Email,
            Address = request.Address,
        };

        await _crecheRepository.UpsertAsync(creche);

        return new BaseResponse<CreatedCrecheResponse>()
        {
            Message = ReturnMessages.CRECHE_REGISTERED_SUCCESSFULLY,
            StatusCode = HttpStatusCode.Created,
            Data = new CreatedCrecheResponse(creche.Identifier, creche.Name)
        };
    }
}