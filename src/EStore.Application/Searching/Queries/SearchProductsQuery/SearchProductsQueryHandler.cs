using EStore.Application.Common.Searching;
using EStore.Contracts.Searching;
using MediatR;

namespace EStore.Application.Searching.Queries.SearchProductsQuery;

public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, SearchProductListPagedResponse>
{
    private readonly ISearchProductsService _searchProductsService;

    public SearchProductsQueryHandler(ISearchProductsService searchProductsService)
    {
        _searchProductsService = searchProductsService;
    }

    public async Task<SearchProductListPagedResponse> Handle(
        SearchProductsQuery request,
        CancellationToken cancellationToken)
    {
        return await _searchProductsService.SearchProductsAsync(
            request.SearchQuery,
            request.Page,
            request.PageSize);
    }
}
