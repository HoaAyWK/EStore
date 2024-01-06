using EStore.Contracts.Searching;

namespace EStore.Application.Common.Searching;

public interface ISearchProductsService
{
    Task<SearchProductListPagedResponse> SearchProductsAsync(
        string? searchQuery,
        int page,
        int pageSize);
}
