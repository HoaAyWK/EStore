using EStore.Domain.Catalog.CategoryAggregate.ValueObjects;

namespace EStore.Domain.Catalog.CategoryAggregate.Repositories;

public interface ICategoryReadRepository
{
    Task<List<Category>> GetAllAsync();
    Task<List<Category>> GetParentCategoryWithChildren();
    Task<Category?> GetByIdAsync(CategoryId categoryId);
}
