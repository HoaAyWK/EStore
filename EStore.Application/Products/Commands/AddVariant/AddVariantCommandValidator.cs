using FluentValidation;

namespace EStore.Application.Products.Commands.AddVariant;

public class AddVariantCommandValidator
    : AbstractValidator<AddVariantCommand>
{
    public AddVariantCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();

        RuleFor(x => x.AttributeSelections).NotEmpty();

        RuleForEach(x => x.AttributeSelections).NotEmpty();

        RuleForEach(x => x.AssignedImageIds).Must(id => id != Guid.Empty)
            .WithMessage("Invalid product's image identifier format");

        RuleForEach(x => x.AttributeSelections).ChildRules(selection =>
        {
            selection.RuleFor(x => x.ProductAttributeId).NotEmpty();

            selection.RuleFor(x => x.ProductAttributeValueId).NotEmpty();
        });

        RuleFor(x => x.Price).NotNull();

        RuleFor(x => x.StockQuantity).NotNull();
    }
}
