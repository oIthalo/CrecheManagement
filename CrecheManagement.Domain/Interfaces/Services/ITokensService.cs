namespace CrecheManagement.Domain.Interfaces.Services;

public interface ITokensService
{
    string GenerateAccessToken(string userIdentifier);
    string GenerateRefreshToken();
    string ValidateTokenAndGetUserIdentifier(string token);
}