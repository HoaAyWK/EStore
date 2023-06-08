using FluentValidation;

namespace EStore.Application.Products.Commands.AddProductAttribute;

public class AddProductAttributeCommandValidator
    : AbstractValidator<AddProductAttributeCommand>
{
    public AddProductAttributeCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();

        RuleFor(x => x.Name).NotEmpty();
    }
}
