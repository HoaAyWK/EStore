using EStore.Domain.CategoryAggregate.ValueObjects;

namespace EStore.Domain.CategoryAggregate.Repositories;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task<Category?> GetByIdAsync(CategoryId id);
    void Delete(Category category);
}
