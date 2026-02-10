using System.Net;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Interfaces.Providers;
using CrecheManagement.Domain.Messages;

namespace CrecheManagement.API.Providers;

public class TokenProvider : ITokenProvider
{
    private readonly IHttpContextAccessor _context;

    public TokenProvider(IHttpContextAccessor context)
    {
        _context = context;
    }

    public string GetToken()
    {
        var authorization = _context.HttpContext?.Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(authorization))
            throw new CrecheManagementException(ReturnMessages.AUTHORIZATION_MISSING, HttpStatusCode.Unauthorized);

        return authorization["Bearer ".Length..].Trim();
    }
}