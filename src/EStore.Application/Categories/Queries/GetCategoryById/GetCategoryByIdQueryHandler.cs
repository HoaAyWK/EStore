using ErrorOr;
using EStore.Application.Common.Interfaces.Services;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.CategoryAggregate.Repositories;
using MediatR;

namespace EStore.Application.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler
    : IRequestHandler<GetCategoryByIdQuery, ErrorOr<Category?>>
{
    private readonly ICategoryReadService _categoryReadService;

    public GetCategoryByIdQueryHandler(ICategoryReadService categoryReadService)
    {
        _categoryReadService = categoryReadService;
    }

    public async Task<ErrorOr<Category?>> Handle(
        GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _categoryReadService.GetByIdAsync(request.Id);
    }
}
