using FluentValidation;

namespace EStore.Application.ProductAttributes.Commands.DeleteProductAttribute;

public class DeleteProductAttributeCommandValidator
    : AbstractValidator<DeleteProductAttributeCommand>
{
    public DeleteProductAttributeCommandValidator()
    {
        RuleFor(x => x.ProductAttributeId).NotNull();
    }
}
