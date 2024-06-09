using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EStore.Infrastructure.Services.FileUploads;

public static class DependencyInjection
{
    public static IServiceCollection AddCloudinary(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var options = configuration
            .GetSection(CloudinaryOptions.SectionName)
            .Get<CloudinaryOptions>();

        var account = new Account(
            options!.CloudName,
            options.ApiKey,
            options.ApiSecret);

        var cloudinary = new Cloudinary(account);

        services.AddSingleton(cloudinary);

        return services;
    }
}
