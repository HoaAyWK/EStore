using System.Text;
using EStore.Application.Common.Interfaces.Services;
using EStore.Contracts.Categories;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.Common.Collections;
using MediatR;

namespace EStore.Application.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQueryHandler
    : IRequestHandler<GetAllCategoriesQuery, List<CategoryWithPathResponse>>
{
    private readonly ICategoryReadService _categoryReadService;

    public GetAllCategoriesQueryHandler(ICategoryReadService categoryReadService)
    {
        _categoryReadService = categoryReadService;
    }

    public async Task<List<CategoryWithPathResponse>> Handle(
        GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categoryTree = await _categoryReadService.GetCategoryTreeAsync();
        var categories = categoryTree.Flatten(false);

        return categories.ToList();
    }
}
