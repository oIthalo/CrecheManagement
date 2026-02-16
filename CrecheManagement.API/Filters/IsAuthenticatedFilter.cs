using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Interfaces.Providers;
using CrecheManagement.Domain.Interfaces.Security;
using CrecheManagement.Domain.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace CrecheManagement.API.Filters;

public class IsAuthenticatedFilter : IAuthorizationFilter
{
    private readonly ITokensService _tokensService;
    private readonly ITokenProvider _tokenProvider;

    public IsAuthenticatedFilter(ITokensService tokensService, ITokenProvider tokenProvider)
    {
        _tokensService = tokensService;
        _tokenProvider = tokenProvider;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            var token = _tokenProvider.GetToken();
            _tokensService.ValidateTokenAndGetUserIdentifier(token);
        }
        catch (CrecheManagementException ex)
        {
            context.HttpContext.Response.StatusCode = 401;
            context.Result = new ObjectResult(new ErrorResponse()
            {
                ErrorMessage = ex.Message,
                ErrorCode = ex.ErrorCode,
                StatusCode = (int)ex.StatusCode
            });
        }
        catch (SecurityTokenExpiredException)
        {
            context.HttpContext.Response.StatusCode = 401;
            context.Result = new ObjectResult(new ErrorResponse()
            {
                StatusCode = 401,
                ErrorCode = "UNAUTHORIZED",
                ErrorMessage = "Unauthorized. COD: 004.",
            });
        }
        catch
        {
            context.HttpContext.Response.StatusCode = 401;
            context.Result = new ObjectResult(new ErrorResponse()
            {
                StatusCode = 401,
                ErrorCode = "UNAUTHORIZED",
                ErrorMessage = "Unauthorized. COD: 001.",
            });
        }
    }
}