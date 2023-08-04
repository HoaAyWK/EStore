using FluentValidation;

namespace EStore.Application.Products.Commands.AddProductVariant;

public class AddProductVariantCommandValidator : AbstractValidator<AddProductVariantCommand>
{
    public AddProductVariantCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();

        RuleFor(x => x.SelectedAttributes).NotEmpty();

        RuleForEach(x => x.SelectedAttributes).NotEmpty();

        RuleForEach(x => x.SelectedAttributes).ChildRules(selection =>
        {
            selection.RuleFor(x => x.ProductAttributeId).NotEmpty();

            selection.RuleFor(x => x.ProductAttributeValueId).NotEmpty();
        });

        RuleFor(x => x.StockQuantity).NotNull();
    }
}