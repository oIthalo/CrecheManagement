using CrecheManagement.Domain.Commands.Creche;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using MediatR;

namespace CrecheManagement.Domain.Handlers.Commands.Creche;

public class UpdateCrecheCommandHandler : IRequestHandler<UpdateCrecheCommand>
{
    private readonly ICrechesRepository _crechesRepository;
    private readonly ICrecheAuthorizationService _crecheAuthorizationService;

    public UpdateCrecheCommandHandler(ICrechesRepository crechesRepository, ICrecheAuthorizationService crecheAuthorizationService)
    {
        _crechesRepository = crechesRepository;
        _crecheAuthorizationService = crecheAuthorizationService;
    }

    public async Task Handle(UpdateCrecheCommand request, CancellationToken cancellationToken)
    {
        var creche = await _crecheAuthorizationService.AuthorizeAndGetCreche(request.Identifier!);

        creche.Name = !string.IsNullOrEmpty(request.Name) ? request.Name : creche.Name;
        creche.Email = !string.IsNullOrEmpty(request.Email) ? request.Email : creche.Email;
        creche.ContactNumber = !string.IsNullOrEmpty(request.ContactNumber) ? request.ContactNumber : creche.ContactNumber;
        creche.Address = request.Address ?? creche.Address;

        await _crechesRepository.UpsertAsync(creche);
    }
}