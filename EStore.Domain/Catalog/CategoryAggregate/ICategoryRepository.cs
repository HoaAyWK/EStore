using EStore.Domain.Catalog.CategoryAggregate.ValueObjects;

namespace EStore.Domain.Catalog.CategoryAggregate;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task<Category?> GetByIdAsync(CategoryId id);
}
