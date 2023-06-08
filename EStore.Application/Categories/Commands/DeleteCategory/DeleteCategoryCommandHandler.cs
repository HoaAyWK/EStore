using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Catalog.CategoryAggregate.Repositories;
using EStore.Domain.Catalog.ProductAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler
    : IRequestHandler<DeleteCategoryCommand, ErrorOr<Deleted>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IProductReadRepository productReadRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _productReadRepository = productReadRepository;
        _unitOfWork = unitOfWork;
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

        if (await _productReadRepository.AnyProductWithCategoryId(request.Id))
        {
            return Errors.Category.AlreadyContainedProducts;
        }

        _categoryRepository.Delete(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Deleted;
    }
}
