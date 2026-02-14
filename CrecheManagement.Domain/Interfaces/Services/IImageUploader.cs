using CrecheManagement.Domain.Dtos;

namespace CrecheManagement.Domain.Interfaces.Services;

public interface IImageUploader
{
    Task<string> UploadImageAsync(ImageUploadDto request);
}