namespace CrecheManagement.Domain.Interfaces.Security;

public interface ITokensService
{
    string GenerateAccessToken(string userIdentifier);
    string GenerateRefreshToken();
    string ValidateTokenAndGetUserIdentifier(string token);
}