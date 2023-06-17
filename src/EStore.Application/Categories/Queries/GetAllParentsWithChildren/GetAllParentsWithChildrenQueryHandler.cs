using EStore.Application.Common.Interfaces.Services;
using EStore.Domain.CategoryAggregate;
using MediatR;

namespace EStore.Application.Categories.Queries.GetAllParentsWithChildren;

public class GetAllParentsWithChildrenQueryHandler
    : IRequestHandler<GetAllParentsWithChildrenQuery, List<Category>>
{
    private readonly ICategoryReadService _categoryReadService;

    public GetAllParentsWithChildrenQueryHandler(ICategoryReadService categoryReadService)
    {
        _categoryReadService = categoryReadService;
    }

    public async Task<List<Category>> Handle(
        GetAllParentsWithChildrenQuery request,
        CancellationToken cancellationToken)
    {
        return await _categoryReadService.GetAllParentsWithChildrenAsync();
    }
}
