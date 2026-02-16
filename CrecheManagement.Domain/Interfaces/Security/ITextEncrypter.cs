namespace CrecheManagement.Domain.Interfaces.Security;

public interface ITextEncrypter
{
    string Encrypt(string text);
    bool IsValid(string text, string textHash);
}