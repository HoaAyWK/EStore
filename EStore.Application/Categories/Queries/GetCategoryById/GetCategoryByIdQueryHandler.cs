using ErrorOr;
using EStore.Domain.Catalog.CategoryAggregate;
using EStore.Domain.Catalog.CategoryAggregate.Repositories;
using EStore.Domain.Catalog.CategoryAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler
    : IRequestHandler<GetCategoryByIdQuery, ErrorOr<Category?>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ErrorOr<Category?>> Handle(
        GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _categoryRepository.GetByIdAsync(CategoryId.Create(request.Id));
    }
}
