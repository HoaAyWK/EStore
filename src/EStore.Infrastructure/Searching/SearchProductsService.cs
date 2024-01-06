using Algolia.Search.Clients;
using Algolia.Search.Models.Search;
using EStore.Application.Common.Searching;
using EStore.Contracts.Searching;
using EStore.Infrastructure.Services.AlgoliaSearch.Options;
using Microsoft.Extensions.Options;

namespace EStore.Infrastructure.Searching;

public class SearchProductsService : ISearchProductsService
{
    private readonly ISearchClient _searchClient;
    private readonly AlgoliaSearchOptions _algoliaSearchOptions;

    public SearchProductsService(
        ISearchClient searchClient,
        IOptions<AlgoliaSearchOptions> options)
    {
        _searchClient = searchClient;
        _algoliaSearchOptions = options.Value;
    }

    public async Task<SearchProductListPagedResponse> SearchProductsAsync(
        string? searchQuery,
        int page,
        int pageSize)
    {
        var index = _searchClient.InitIndex(_algoliaSearchOptions.IndexName);

        var query = new Query(searchQuery)
        {
            Page = page,
            HitsPerPage = pageSize
        };

        var result = await index.SearchAsync<SearchProductResponse>(query);

        return new SearchProductListPagedResponse
        {
            Hits = result.Hits,
            Page = result.Page,
            PageSize = result.HitsPerPage,
            TotalHits = result.NbHits,
            TotalPages = result.NbPages
        };
    }
}
