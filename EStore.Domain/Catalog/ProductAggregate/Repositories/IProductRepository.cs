using EStore.Domain.Catalog.ProductAggregate.ValueObjects;

namespace EStore.Domain.Catalog.ProductAggregate.Repositories;

public interface IProductRepository
{
    Task AddAsync(Product product);
    Task<Product?> GetByIdAsync(ProductId productId);
}
