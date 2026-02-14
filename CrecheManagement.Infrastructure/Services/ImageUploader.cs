using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CrecheManagement.Domain.Dtos;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using Microsoft.Extensions.Configuration;

namespace CrecheManagement.Infrastructure.Services;

public class ImageUploader : IImageUploader
{
    private Cloudinary _cloudinary;

    private string _cloudName;
    private string _apiSecret;
    private string _apiKey;

    public ImageUploader(IConfiguration configuration)
    {
        _cloudName = configuration["External:Cloudinary:CloudName"]!;
        _apiSecret = configuration["External:Cloudinary:API_Secret"]!;
        _apiKey = configuration["External:Cloudinary:API_Key"]!;

        var account = new Account(_cloudName, _apiKey, _apiSecret);
        _cloudinary = new Cloudinary(account);

        _cloudinary.Api.Secure = true;
    }

    public async Task<string> UploadImageAsync(ImageUploadDto request)
    {
        var file = request.File;

        var fileName = file.FileName;
        await using var stream = file.OpenReadStream();

        var imageParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, stream),
            Folder = request.Folder,
            Overwrite = true,
            PublicId = $"{request.PublicId}_{Guid.NewGuid().ToString("N")}",
        };

        var result = await _cloudinary.UploadAsync(imageParams);
        if (result.StatusCode != HttpStatusCode.OK)
            throw new CrecheManagementException(ReturnMessages.DEFAULT_INTERNAL_ERROR, HttpStatusCode.InternalServerError);

        return result.SecureUrl.AbsoluteUri;
    }
}