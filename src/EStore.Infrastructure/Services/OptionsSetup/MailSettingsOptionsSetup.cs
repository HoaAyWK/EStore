using EStore.Infrastructure.Services.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.Services.OptionsSetup;

public class MailSettingsOptionsSetup : IConfigureOptions<MailSettings>
{
    private readonly IConfiguration _configuration;

    public MailSettingsOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(MailSettings options)
    {
        _configuration.GetSection(MailSettings.SectionName).Bind(options);
    }
}
