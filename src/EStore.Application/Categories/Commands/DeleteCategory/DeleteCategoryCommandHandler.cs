using ErrorOr;
using EStore.Domain.CategoryAggregate.Repositories;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler
    : IRequestHandler<DeleteCategoryCommand, ErrorOr<Deleted>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;

    public DeleteCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IProductRepository productRepository)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(
        DeleteCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);

        if (category is null)
        {
            return Errors.Category.NotFound;
        }

        if (category.Children.Count > 0)
        {
            return Errors.Category.AlreadyContainedChildren;
        }

        if (await _productRepository.AnyProductWithCategoryId(request.Id))
        {
            return Errors.Category.AlreadyContainedProducts;
        }

        _categoryRepository.Delete(category);
                
        return Result.Deleted;
    }
}
