using EStore.Application.Common.Dtos;
using EStore.Contracts.Categories;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.Common.Collections;

namespace EStore.Application.Common.Interfaces.Services;

public interface ICategoryReadService
{
    Task<Category?> GetByIdAsync(CategoryId categoryId);
    Task<CategoryWithPathsResponse?> GetBySlug(string slug);
    Task<ListPagedCategoryResult> GetListPagedAsync(int pageSize = 0, int page = 0);
    Task<List<Category>> GetAllParentsWithChildrenAsync();
    Task<List<Category>> GetCategoriesWithParentAsync();
    Task<TreeNode<CategoryWithPathResponse>> GetCategoryTreeAsync();
}
