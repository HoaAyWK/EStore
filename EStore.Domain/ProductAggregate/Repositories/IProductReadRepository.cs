using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.ProductAggregate.Repositories;

public interface IProductReadRepository
{
    Task<Product?> GetByIdAsync(ProductId productId);
    Task<List<Product>> GetAllAsync();
    Task<List<Product>> GetAllWithBrandAndCategory();
    Task<bool> AnyProductWithBrandId(BrandId brandId);
    Task<bool> AnyProductWithCategoryId(CategoryId categoryId);
}
