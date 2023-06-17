using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.Authentication.OptionsSetup;

public class JwtSettingsOptionsSetup : IConfigureOptions<JwtSettings>
{
    private readonly IConfiguration _configuration;

    public JwtSettingsOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(JwtSettings options)
    {
        _configuration.GetSection(JwtSettings.SectionName).Bind(options);
    }
}
