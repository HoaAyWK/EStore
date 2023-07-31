using EStore.Domain.CategoryAggregate;
using FluentValidation;

namespace EStore.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .MinimumLength(Category.MinNameLength)
            .MaximumLength(Category.MaxNameLength);
    }
}
