using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.Services.FileUploads.OptionsSetup;

public class CloudinaryOptionsSetup : IConfigureOptions<CloudinaryOptions>
{
    private readonly IConfiguration _configuration;

    public CloudinaryOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(CloudinaryOptions options)
    {
        _configuration.GetSection(CloudinaryOptions.SectionName).Bind(options);
    }
}
