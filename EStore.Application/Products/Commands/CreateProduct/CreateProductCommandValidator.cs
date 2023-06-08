using FluentValidation;

namespace EStore.Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.Description).NotEmpty();

        RuleFor(x => x.BrandId).NotNull();

        RuleFor(x => x.CategoryId).NotNull();

        RuleFor(x => x.Published).NotNull();
    }
}

