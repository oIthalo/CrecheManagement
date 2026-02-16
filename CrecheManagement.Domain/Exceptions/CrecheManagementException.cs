using System.Collections;
using System.Globalization;
using System.Net;
using CrecheManagement.Domain.Messages;

namespace CrecheManagement.Domain.Exceptions;

public class CrecheManagementException : Exception
{
    public HttpStatusCode StatusCode { get; set; }
    public string? ErrorCode { get; set; }

    public CrecheManagementException(string message, HttpStatusCode statusCode) 
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = GetErrorCode(message);
    }

    private string? GetErrorCode(string errorMessage)
    {
        var resourceSet = ReturnMessages.ResourceManager
            .GetResourceSet(CultureInfo.CurrentUICulture, true, true);

        foreach (DictionaryEntry entry in resourceSet!)
        {
            if (entry.Value?.ToString() == errorMessage)
                return entry.Key.ToString();
        }

        return null;
    }
}