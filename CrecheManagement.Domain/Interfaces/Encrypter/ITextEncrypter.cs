namespace CrecheManagement.Domain.Interfaces.Encrypter;

public interface ITextEncrypter
{
    string Encrypt(string text);
    bool IsValid(string text, string textHash);
}