using FluentValidation;

namespace EStore.Application.Products.Commands.AddProductAttributeValue;

public class AddProductAttributeValueCommandValidator
    : AbstractValidator<AddProductAttributeValueCommand>
{
    public AddProductAttributeValueCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        
        RuleFor(x => x.ProductId).NotEmpty();

        RuleFor(x => x.ProductAttributeId).NotEmpty();
    }
}
