using ErrorOr;
using MediatR;
using EStore.Domain.Common.Errors;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.CategoryAggregate.Repositories;

namespace EStore.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ErrorOr<Category>>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ErrorOr<Category>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var existingCategory = await _categoryRepository.GetBySlugAsync(request.Slug);

        if (existingCategory is not null)
        {
            return Errors.Category.CategoryWithProvidedSlugAlreadyExists(request.Slug);
        }

        var createCategoryResult = Category.Create(
            request.Name,
            request.Slug,
            request.ImageUrl,
            request.ParentId);

        if (createCategoryResult.IsError)
        {
            return createCategoryResult.Errors;
        }

        var category = createCategoryResult.Value;

        if (request.ParentId is not null)
        {
            var parentCategory = await _categoryRepository.GetByIdAsync(request.ParentId);

            if (parentCategory is null)
            {
                return Errors.Category.ParentCategoryNotFound;
            }
        }
        
        await _categoryRepository.AddAsync(category);

        return category;
    }
}
