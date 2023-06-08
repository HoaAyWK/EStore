using EStore.Domain.CategoryAggregate;
using EStore.Domain.CategoryAggregate.Repositories;
using MediatR;

namespace EStore.Application.Categories.Queries.GetParentCategoryWithChildren;

public class GetParentCategoryWithChildrenQueryHandler
    : IRequestHandler<GetParentCategoryWithChildrenQuery, List<Category>>
{
    private readonly ICategoryReadRepository _categoryReadRepository;

    public GetParentCategoryWithChildrenQueryHandler(ICategoryReadRepository categoryReadRepository)
    {
        _categoryReadRepository = categoryReadRepository;
    }

    public async Task<List<Category>> Handle(
        GetParentCategoryWithChildrenQuery request,
        CancellationToken cancellationToken)
    {
        return await _categoryReadRepository.GetParentCategoryWithChildren();
    }
}
