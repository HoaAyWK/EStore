using EStore.Application.Common.Dtos;
using EStore.Application.Common.Interfaces.Services;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.CategoryAggregate.Repositories;
using MediatR;

namespace EStore.Application.Categories.Queries.GetCategoryListPaged;

public class GetCategoryListPagedQueryHandler
    : IRequestHandler<GetCategoryListPagedQuery, ListPagedCategoryResult>
{
    private readonly ICategoryReadService _categoryReadService;

    public GetCategoryListPagedQueryHandler(ICategoryReadService categoryReadService)
    {
        _categoryReadService = categoryReadService;
    }

    public async Task<ListPagedCategoryResult> Handle(
        GetCategoryListPagedQuery request,
        CancellationToken cancellationToken)
    {
        return await _categoryReadService.GetListPagedAsync(request.PageSize, request.Page);
    }
}
