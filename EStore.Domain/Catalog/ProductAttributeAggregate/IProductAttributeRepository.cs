namespace EStore.Domain.Catalog.ProductAttributeAggregate;

public interface IProductAttributeRepository
{
    Task AddAsync(ProductAttribute productAttribute);
    void Delete(ProductAttribute productAttribute);
}