using Algolia.Search.Clients;
using Algolia.Search.Models.Settings;
using EStore.Infrastructure.Services.AlgoliaSearch.Interfaces;
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.Services.AlgoliaSearch.Services;

public class AlgoliaIndexSettingsService : IAlgoliaIndexSettingsService
{
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;

    public AlgoliaIndexSettingsService(
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> algoliaSearchOptions)
    {
        _searchClient = searchClient;
        _algoliaSearchOptions = algoliaSearchOptions.Value;
    }

    public async Task<IndexSettings> GetIndexSettingsAsync()
    {
        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);
        
        return await index.GetSettingsAsync();
    }
}
