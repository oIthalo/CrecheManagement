using System.Text.Json.Serialization;

namespace CrecheManagement.Domain.Utils;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string ErrorMessage { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<ErrorValidation> Errors { get; set; } = new();
}

public class ErrorValidation
{
    public string Field { get; set; }
    public string Message { get; set; }
}