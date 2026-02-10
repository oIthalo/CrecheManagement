using System.Net;

namespace CrecheManagement.Domain.Utils;

public class BaseResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; }
}

public class BaseResponse<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}