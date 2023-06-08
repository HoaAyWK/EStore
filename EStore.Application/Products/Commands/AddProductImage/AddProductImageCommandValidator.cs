using FluentValidation;

namespace EStore.Application.Products.Commands.AddProductImage;

public class AddProductImageCommandValidator : AbstractValidator<AddProductImageCommand>
{
    public AddProductImageCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();

        RuleFor(x => x.ImageUrl).NotEmpty();

        RuleFor(x => x.DisplayOrder).NotNull();
    }
}
