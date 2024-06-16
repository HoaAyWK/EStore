using ErrorOr;
using EStore.Application.Common.Interfaces.Services;
using EStore.Domain.Common.Errors;
using EStore.Contracts.Categories;
using EStore.Domain.CategoryAggregate;
using MediatR;

namespace EStore.Application.Categories.Queries.GetCategoryBySlug;

public class GetCategoryBySlugQueryHandler
    : IRequestHandler<GetCategoryBySlugQuery, ErrorOr<CategoryWithPathsResponse>>
{
    private readonly ICategoryReadService _categoryReadService;

    public GetCategoryBySlugQueryHandler(ICategoryReadService categoryReadService)
        => _categoryReadService = categoryReadService;

    public async Task<ErrorOr<CategoryWithPathsResponse>> Handle(
        GetCategoryBySlugQuery request,
        CancellationToken cancellationToken)
    {
        var category = await _categoryReadService.GetBySlug(request.Slug);

        if (category is null)
        {
            return Errors.Category.NotFound;
        }

        return category;
    }
}
