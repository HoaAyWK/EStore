using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.Persistence.Seeds;

public class UserSeedingOptionsSetup : IConfigureOptions<UserSeedingSettings>
{
    private readonly IConfiguration _configuration;

    public UserSeedingOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(UserSeedingSettings options)
    {
        _configuration.GetSection(UserSeedingSettings.SectionName).Bind(options);
    }
}
