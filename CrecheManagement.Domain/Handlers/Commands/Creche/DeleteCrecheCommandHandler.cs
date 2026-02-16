using CrecheManagement.Domain.Commands.Creche;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using MediatR;

namespace CrecheManagement.Domain.Handlers.Commands.Creche;

public class DeleteCrecheCommandHandler : IRequestHandler<DeleteCrecheCommand>
{
    private readonly ICrecheAuthorizationService _crecheAuthorizationService;
    private readonly ICrechesRepository _crechesRepository;

    public DeleteCrecheCommandHandler(ICrecheAuthorizationService crecheAuthorizationService, ICrechesRepository crechesRepository)
    {
        _crecheAuthorizationService = crecheAuthorizationService;
        _crechesRepository = crechesRepository;
    }

    public async Task Handle(DeleteCrecheCommand request, CancellationToken cancellationToken)
    {
        var creche = await _crecheAuthorizationService.AuthorizeAndGetCreche(request.Identifier!);

        await _crechesRepository.DeleteAsync(creche);
    }
}