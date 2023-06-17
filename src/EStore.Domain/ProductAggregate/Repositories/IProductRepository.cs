using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.ProductAggregate.Repositories;

public interface IProductRepository
{
    Task AddAsync(Product product);
    Task<Product?> GetByIdAsync(ProductId productId);
    Task<bool> AnyProductWithCategoryId(CategoryId categoryId);

    Task<bool> AnyProductWithBrandId(BrandId brandId);
}
