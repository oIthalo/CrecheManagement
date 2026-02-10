using System.Net;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Interfaces.Providers;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Models;
using CrecheManagement.Infrastructure.Context;
using MongoDB.Driver;

namespace CrecheManagement.Infrastructure.Security;

public class LoggedUserService : ILoggedUserService
{
    private readonly ITokenProvider _tokenProvider;
    private readonly ITokensService _tokensService;
    private readonly MongoContext _mongo;

    public LoggedUserService(
        ITokenProvider tokenProvider, 
        ITokensService tokensService, 
        MongoContext mongo)
    {
        _tokenProvider = tokenProvider;
        _tokensService = tokensService;
        _mongo = mongo;
    }

    public async Task<User> GetUser()
    {
        var token = _tokenProvider.GetToken();
        var identifier = _tokensService.ValidateTokenAndGetUserIdentifier(token);

        if (string.IsNullOrEmpty(identifier))
            throw new CrecheManagementException(ReturnMessages.AUTHORIZATION_MISSING, HttpStatusCode.Unauthorized);

        var filter = Builders<User>.Filter.Eq(x => x.Identifier, identifier);

        var user = await _mongo.Users.Find(filter).FirstOrDefaultAsync() ?? 
            throw new CrecheManagementException("Unauthorized. COD: 003", HttpStatusCode.Unauthorized);

        return user;
    }
}