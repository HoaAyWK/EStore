using EStore.Domain.Catalog.ProductAggregate.ValueObjects;

namespace EStore.Domain.Catalog.ProductAggregate;

public interface IProductReadRepository
{
    Task<Product?> GetByIdAsync(ProductId productId);
}
