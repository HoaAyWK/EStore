using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.CategoryAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler
    : IRequestHandler<UpdateCategoryCommand, ErrorOr<Updated>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);

        if (category is null)
        {
            return Errors.Category.NotFound;
        }

        var updateCategoryNameResult = category.UpdateName(request.Name);

        if (updateCategoryNameResult.IsError)
        {
            return updateCategoryNameResult.Errors;
        }

        if (request.ParentId is not null)
        {
            var parent = await _categoryRepository.GetByIdAsync(request.ParentId);

            if (parent is null)
            {
                return Errors.Category.ParentCategoryNotFound;
            }

            category.UpdateParentCategory(request.ParentId);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}
