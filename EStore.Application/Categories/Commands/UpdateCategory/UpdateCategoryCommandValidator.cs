using FluentValidation;

namespace EStore.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator
    : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name).NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);
    }
}
