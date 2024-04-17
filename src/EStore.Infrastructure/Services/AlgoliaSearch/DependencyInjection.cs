using Algolia.Search.Clients;
using EStore.Infrastructure.Services.AlgoliaSearch.Interfaces;
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using EStore.Infrastructure.Services.AlgoliaSearch.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EStore.Infrastructure.Services.AlgoliaSearch;

public static class DependencyInjection
{
    public static IServiceCollection AddAlgoliaSearch(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var optionsSection = configuration
            .GetSection(AlgoliaSearchOptions.SectionName);
        
        var options = optionsSection.Get<AlgoliaSearchOptions>();

        services.Configure<AlgoliaSearchOptions>(optionsSection);

        // services.AddSingleton<ISearchClient, SearchClient>();
        services.AddSingleton<ISearchClient>(
            new SearchClient(
                options!.ApplicationId,
                options.AdminApiKey));

        services.AddScoped<IAlgoliaIndexSettingsService, AlgoliaIndexSettingsService>();

        return services;
    }
}
