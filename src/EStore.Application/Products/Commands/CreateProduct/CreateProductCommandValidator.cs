using EStore.Domain.ProductAggregate;
using FluentValidation;

namespace EStore.Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .MinimumLength(Product.MinNameLength)
            .MaximumLength(Product.MaxNameLength);

        RuleFor(x => x.Description).NotEmpty();

        RuleFor(x => x.Published).NotNull();

        RuleFor(x => x.BrandId).NotEmpty();

        RuleFor(x => x.CategoryId).NotEmpty();
    }
}

