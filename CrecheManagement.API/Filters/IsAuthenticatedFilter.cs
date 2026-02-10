using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Interfaces.Providers;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
                StatusCode = (int)ex.StatusCode
            });
        }
        catch
        {
            context.HttpContext.Response.StatusCode = 401;
            context.Result = new ObjectResult(new ErrorResponse()
            {
                StatusCode = 401,
                ErrorMessage = "Unauthorized. COD: 001.",
            });
        }
    }
}