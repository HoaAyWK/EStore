using FluentValidation;

namespace EStore.Application.Products.Commands.UpdateProductAttribute;

public class UpdateProductAttributeCommandValidator
    : AbstractValidator<UpdateProductAttributeCommand>
{
    public UpdateProductAttributeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.ProductId).NotEmpty();

        RuleFor(x => x.Name).NotEmpty();
    }
}
