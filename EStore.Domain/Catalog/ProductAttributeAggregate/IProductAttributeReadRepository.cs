using EStore.Domain.Catalog.ProductAttributeAggregate.ValueObjects;

namespace EStore.Domain.Catalog.ProductAttributeAggregate;

public interface IProductAttributeReadRepository
{
    Task<List<ProductAttribute>> GetAllAsync();
    Task<ProductAttribute?> GetByIdAsync(ProductAttributeId productAttributeId);
}
