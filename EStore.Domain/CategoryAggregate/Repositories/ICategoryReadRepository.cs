using EStore.Domain.CategoryAggregate.ValueObjects;

namespace EStore.Domain.CategoryAggregate.Repositories;

public interface ICategoryReadRepository
{
    Task<List<Category>> GetAllAsync();
    Task<List<Category>> GetParentCategoryWithChildren();
    Task<Category?> GetByIdAsync(CategoryId categoryId);
}
