using Microsoft.AspNetCore.Http;

namespace CrecheManagement.Domain.Dtos;

public record ImageUploadDto
{
    public IFormFile File { get; init; }
    public string Folder { get; init; }
    public string PublicId { get; init; }
}