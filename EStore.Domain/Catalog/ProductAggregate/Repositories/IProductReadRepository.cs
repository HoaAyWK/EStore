using EStore.Domain.Catalog.BrandAggregate.ValueObjects;
using EStore.Domain.Catalog.CategoryAggregate.ValueObjects;
using EStore.Domain.Catalog.ProductAggregate.ValueObjects;

namespace EStore.Domain.Catalog.ProductAggregate.Repositories;

public interface IProductReadRepository
{
    Task<Product?> GetByIdAsync(ProductId productId);
    Task<List<Product>> GetAllAsync();
    Task<List<Product>> GetAllWithBrandAndCategory();
    Task<bool> AnyProductWithBrandId(BrandId brandId);
    Task<bool> AnyProductWithCategoryId(CategoryId categoryId);
}
