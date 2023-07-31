using EStore.Domain.CategoryAggregate;
using FluentValidation;

namespace EStore.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator
    : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name).NotEmpty()
            .MinimumLength(Category.MinNameLength)
            .MaximumLength(Category.MaxNameLength);
    }
}
