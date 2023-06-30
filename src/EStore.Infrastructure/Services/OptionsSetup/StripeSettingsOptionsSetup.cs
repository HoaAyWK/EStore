using EStore.Infrastructure.Services.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.Services.OptionsSetup;

public class StripeSettingsOptionsSetup : IConfigureOptions<StripeSettings>
{
    private readonly IConfiguration _configuration;

    public StripeSettingsOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(StripeSettings options)
    {
        _configuration.GetSection(StripeSettings.SectionName).Bind(options);
    }
}
