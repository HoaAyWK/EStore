using EStore.Domain.CategoryAggregate;
using EStore.Domain.CategoryAggregate.Repositories;
using MediatR;

namespace EStore.Application.Categories.Queries.GetCategoryListPaged;

public class GetCategoryListPagedQueryHandler
    : IRequestHandler<GetCategoryListPagedQuery, List<Category>>
{
    private readonly ICategoryReadRepository _categoryReadRepository;

    public GetCategoryListPagedQueryHandler(ICategoryReadRepository categoryReadRepository)
    {
        _categoryReadRepository = categoryReadRepository;
    }

    public async Task<List<Category>> Handle(
        GetCategoryListPagedQuery request,
        CancellationToken cancellationToken)
    {
        return await _categoryReadRepository.GetAllAsync();
    }
}
