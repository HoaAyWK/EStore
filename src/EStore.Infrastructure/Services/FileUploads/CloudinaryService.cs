using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EStore.Application.Common.Interfaces.Services;

namespace EStore.Infrastructure.Services.FileUploads;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<string> UploadFileAsync(
        byte[] fileBytes,
        string fileName)
    {
        using var stream = new MemoryStream(fileBytes);
        
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(fileName, stream),
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return uploadResult.SecureUrl.ToString();
    }
}
