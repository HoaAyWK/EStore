using FluentValidation;

namespace EStore.Application.Products.Commands.UpdateVariant;

public class UpdateVariantCommandValidator
    : AbstractValidator<UpdateVariantCommand>
{
    public UpdateVariantCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();

        RuleFor(x => x.ProductVariantId).NotEmpty();
    }
}
