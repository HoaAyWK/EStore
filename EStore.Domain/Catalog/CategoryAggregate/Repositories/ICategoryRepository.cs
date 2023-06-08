using EStore.Domain.Catalog.CategoryAggregate.ValueObjects;

namespace EStore.Domain.Catalog.CategoryAggregate.Repositories;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task<Category?> GetByIdAsync(CategoryId id);
    void Delete(Category category);
}
