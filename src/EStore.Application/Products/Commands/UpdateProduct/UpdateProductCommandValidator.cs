using EStore.Domain.ProductAggregate;
using FluentValidation;

namespace EStore.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name).NotEmpty()
            .MinimumLength(Product.MinNameLength)
            .MaximumLength(Product.MaxNameLength);

        RuleFor(x => x.ShortDescription).NotEmpty()
            .MinimumLength(Product.MinShortDescriptionLength)
            .MaximumLength(Product.MaxShortDescriptionLength);

        RuleFor(x => x.Description).NotEmpty();

        RuleFor(x => x.Published).NotNull();

        RuleFor(x => x.Price).NotNull()
            .GreaterThanOrEqualTo(Product.MinPrice);

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(Product.MinStockQuantity);
    }
}
