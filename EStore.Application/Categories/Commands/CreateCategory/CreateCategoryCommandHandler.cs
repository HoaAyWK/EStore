using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using MediatR;
using EStore.Domain.Common.Errors;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.CategoryAggregate.Repositories;

namespace EStore.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ErrorOr<Category>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Category>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = Category.Create(request.Name, request.ParentId);

        if (request.ParentId is not null)
        {
            var parentCategory = await _categoryRepository.GetByIdAsync(request.ParentId);

            if (parentCategory is null)
            {
                return Errors.Category.ParentCategoryNotFound;
            }

            parentCategory.AddChildCategory(category);
        }
        
        await _categoryRepository.AddAsync(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return category;
    }
}
