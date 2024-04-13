using EStore.Domain.CategoryAggregate;
using EStore.Domain.CategoryAggregate.Repositories;
using EStore.Domain.CategoryAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Persistence.Repositories;

internal sealed class CategoryRepository : ICategoryRepository
{
    private readonly EStoreDbContext _dbContext;

    public CategoryRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Category category)
    {
        await _dbContext.Categories.AddAsync(category);
    }

    public async Task<Category?> GetByIdAsync(CategoryId id)
    {
        return await _dbContext.Categories.FindAsync(id);
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _dbContext.Categories
            .Where(category => category.Name == name)
            .SingleOrDefaultAsync();
    }

    public async Task<Category?> GetBySlugAsync(string slug)
    {
        return await _dbContext.Categories
            .Where(category => category.Slug == slug)
            .SingleOrDefaultAsync();
    }

    public void Delete(Category category)
    {
        _dbContext.Categories.Remove(category);
    }

    public async Task<Category?> GetWithParentsByIdAsync(CategoryId id)
    {
        return await _dbContext.Categories
            .Include(category => category.Parent)
            .Where(category => category.Id == id)
            .SingleOrDefaultAsync();
    }
}
