using FluentValidation;

namespace EStore.Application.ProductAttributes.Commands.UpdateProductAttribute;

public class UpdateProductAttributeCommandValidator
    : AbstractValidator<UpdateProductAttributeCommand>
{
    public UpdateProductAttributeCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
