using EStore.Domain.CategoryAggregate;
using EStore.Domain.CategoryAggregate.Repositories;
using EStore.Domain.CategoryAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Persistence.Repositories;

internal sealed class CategoryReadRepository : ICategoryReadRepository
{
    private readonly EStoreDbContext _dbContext;

    public CategoryReadRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _dbContext.Categories.AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Category>> GetParentCategoryWithChildren()
    {
        return await _dbContext.Categories.AsNoTracking()
            .Where(c => c.ParentId == null)
            .Include(c => c.Children)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(CategoryId categoryId)
    {
        return await _dbContext.Categories.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == categoryId);
    }
}
