using FluentValidation;

namespace EStore.Application.ProductVariants.Commands.CreateProductVariant;

public class CreateProductVariantCommandValidator
    : AbstractValidator<CreateProductVariantCommand>
{
    public CreateProductVariantCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();

        RuleFor(x => x.SelectedAttributes).NotEmpty();

        RuleForEach(x => x.SelectedAttributes).NotEmpty();

        RuleForEach(x => x.SelectedAttributes).ChildRules(selection =>
        {
            selection.RuleFor(x => x.ProductAttributeId).NotEmpty();

            selection.RuleFor(x => x.ProductAttributeValueId).NotEmpty();
        });

        RuleFor(x => x.Price).NotNull();

        RuleFor(x => x.StockQuantity).NotNull();
    }
}
