using CrecheManagement.Domain.Interfaces.Security;

namespace CrecheManagement.Infrastructure.Security;

public class BCryptNet : ITextEncrypter
{
    public string Encrypt(string text)
    {
        return BCrypt.Net.BCrypt.HashPassword(text);
    }

    public bool IsValid(string text, string textHash)
    {
        return BCrypt.Net.BCrypt.Verify(text, textHash);
    }
}