using Microsoft.AspNetCore.Http;

namespace CrecheManagement.Domain.Dtos;

public class ImageUploadDto
{
    public IFormFile File { get; set; }
    public string Folder { get; set; }
    public string PublicId { get; set; }
}