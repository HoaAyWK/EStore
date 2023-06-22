using EStore.Application.Products.Dtos;
using EStore.Contracts.Common;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Application.Products.Services;

public interface IProductReadService
{
    Task<ProductDto?> GetByIdAsync(ProductId id);

    Task<PagedList<ProductDto>> GetProductListPagedAsync(
        string? searchTerm,
        int page,
        int pageSize);
}
