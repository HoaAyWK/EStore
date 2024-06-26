using EStore.Domain.CategoryAggregate.ValueObjects;

namespace EStore.Domain.CategoryAggregate.Repositories;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task<Category?> GetByIdAsync(CategoryId id);
    Task<Category?> GetByNameAsync(string name);
    Task<Category?> GetBySlugAsync(string slug);
    Task<Category?> GetWithParentsByIdAsync(CategoryId id);
    void Delete(Category category);
}
