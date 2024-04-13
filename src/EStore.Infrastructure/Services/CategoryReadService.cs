using System.Text;
using EStore.Application.Common.Dtos;
using EStore.Application.Common.Interfaces.Services;
using EStore.Contracts.Categories;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.Common.Collections;
using EStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Services;

internal sealed class CategoryReadService : ICategoryReadService
{
    private readonly EStoreDbContext _dbContext;

    public CategoryReadService(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Category?> GetByIdAsync(CategoryId categoryId)
    {
        return await _dbContext.Categories.AsNoTracking()
            .Where(c => c.Id == categoryId)
            .Include(c => c.Parent)
            .Include(c => c.Children)
            .FirstOrDefaultAsync();
    }

    public async Task<ListPagedCategoryResult> GetListPagedAsync(int pageSize, int page)
    {
        if (page is 0 || pageSize is 0)
        {
            return new ListPagedCategoryResult(
                new List<Category>(),
                pageSize,
                page,
                0);
        }

        var totalItems = await _dbContext.Categories.CountAsync();

        var categories = await _dbContext.Categories.AsNoTracking()
            .Include(c => c.Children)
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new ListPagedCategoryResult(
            categories,
            pageSize,
            page,
            totalItems);
    }

    public async Task<List<Category>> GetAllParentsWithChildrenAsync()
    {
        return await _dbContext.Categories.AsNoTracking()
            .Where(c => c.ParentId! == null!)
            .Include(c => c.Children)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<List<Category>> GetCategoriesWithParentAsync()
    {
        return await _dbContext.Categories.AsNoTracking()
            .Where(c => c.ParentId! == null!)
            .Include(c => c.Parent)
            .OrderBy(c => c.Name)
            .SelectMany(c => c.Children)
            .ToListAsync();
    }

    public async Task<TreeNode<CategoryWithPathResponse>> GetCategoryTreeAsync()
    {
        var categories = await _dbContext.Categories
            .AsNoTracking()
            .Include(c => c.Children)
            .Select(c => new CategoryWithPathResponse
            {
                Id = c.Id.Value,
                Name = c.Name,
                Slug = c.Slug,
                ImageUrl = c.ImageUrl,
                Path = "",
                ParentId = c.ParentId! == null! ? null : c.ParentId.Value,
                CreatedDateTime = c.CreatedDateTime,
                UpdatedDateTime = c.UpdatedDateTime,
                HasChild = c.Children.Count > 0
            })
            .ToListAsync();

        var categoriesMap = new MultiMap<Guid, CategoryWithPathResponse>();

        foreach (var category in categories)
        {
            Guid key = category.ParentId! == null! ? Guid.Empty : category.ParentId.Value;
            categoriesMap.Add(key, category);
        }

        var categoryRoot = new CategoryWithPathResponse
        {
            Id = Guid.NewGuid(),
            Name = "Root",
            Path = "Root"
        };

        var rootParent = new TreeNode<CategoryWithPathResponse>(categoryRoot);

        AddChildTreeNodes(rootParent, Guid.Empty, categoriesMap, "");

        return rootParent;
    }

    private void AddChildTreeNodes(
        TreeNode<CategoryWithPathResponse> parentNode,
        Guid parentId,
        MultiMap<Guid, CategoryWithPathResponse> categoriesMap,
        string pathBuffer)
    {
        var categories = categoriesMap.ContainsKey(parentId)
            ? categoriesMap[parentId]
            : Enumerable.Empty<CategoryWithPathResponse>();

        string prevPathBuffer = pathBuffer;

        foreach (var category in categories)
        {
            pathBuffer += category.Name;
            category.Path += pathBuffer;

            if (category.HasChild)
            {
                pathBuffer += " / ";
            }

            var node = new TreeNode<CategoryWithPathResponse>(category);

            parentNode.AddChild(node);
            AddChildTreeNodes(node, category.Id, categoriesMap, pathBuffer);

            pathBuffer = prevPathBuffer;
        }
    }
}
