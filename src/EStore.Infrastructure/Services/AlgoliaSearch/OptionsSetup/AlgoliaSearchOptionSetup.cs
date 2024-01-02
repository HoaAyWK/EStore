using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.Services.AlgoliaSearch.OptionsSetup;

public class AlgoliaSearchOptionSetup : IConfigureOptions<AlgoliaSearchOptions>
{
    private readonly IConfiguration _configuration;

    public AlgoliaSearchOptionSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(AlgoliaSearchOptions options)
    {
        _configuration.GetSection(AlgoliaSearchOptions.SectionName).Bind(options);
    }
}
