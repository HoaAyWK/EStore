using EStore.Domain.ProductVariantAggregate.ValueObjects;

namespace EStore.Domain.ProductVariantAggregate.Repositories;

public interface IProductVariantRepository
{
    Task AddAsync(ProductVariant productVariant);
    Task<ProductVariant?> GetByIdAsync(ProductVariantId id);
    Task<List<ProductVariant>> GetAllAsync();
}
