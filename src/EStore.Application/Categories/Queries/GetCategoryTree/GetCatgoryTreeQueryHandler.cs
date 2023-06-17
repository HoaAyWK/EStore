using EStore.Application.Common.Interfaces.Services;
using EStore.Contracts.Categories;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.Common.Collections;
using MediatR;

namespace EStore.Application.Categories.Queries.GetCategoryTree;

public class GetCategoryTreeQueryHandler
    : IRequestHandler<GetCategoryTreeQuery, List<TreeNode<CategoryWithPathResponse>>>
{
    private readonly ICategoryReadService _categoryReadService;

    public GetCategoryTreeQueryHandler(ICategoryReadService categoryReadService)
    {
        _categoryReadService = categoryReadService;
    }

    public async Task<List<TreeNode<CategoryWithPathResponse>>> Handle(GetCategoryTreeQuery request, CancellationToken cancellationToken)
    {
        var tree = await _categoryReadService.GetCategoryTreeAsync();

        return tree.Children;
    }
}
