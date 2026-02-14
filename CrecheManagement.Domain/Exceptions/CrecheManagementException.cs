using System.Net;

namespace CrecheManagement.Domain.Exceptions;

public class CrecheManagementException : Exception
{
    public HttpStatusCode StatusCode { get; set; }
    public string ErrorCode { get; set; }

    public CrecheManagementException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = nameof(message);
    }
}