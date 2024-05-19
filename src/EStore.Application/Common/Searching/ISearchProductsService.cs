using ErrorOr;
using EStore.Contracts.Searching;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Application.Common.Searching;

public interface ISearchProductsService
{
    Task<SearchProductListPagedResponse> SearchProductsAsync(
        string? searchQuery,
        int page,
        int pageSize);

    Task<ErrorOr<RebuildResult>> RebuildProductAsync(
        ProductId productId,
        ProductVariantId productVariantId);
}
