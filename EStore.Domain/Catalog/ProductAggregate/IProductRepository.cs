using EStore.Domain.Catalog.ProductAggregate.ValueObjects;

namespace EStore.Domain.Catalog.ProductAggregate;

public interface IProductRepository
{
    Task AddAsync(Product product);
    Task<Product?> GetByIdAsync(ProductId id);
}
