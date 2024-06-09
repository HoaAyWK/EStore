namespace EStore.Application.Common.Interfaces.Services;

public interface ICloudinaryService
{
    Task<string> UploadFileAsync(byte[] fileBytes, string fileName);
}