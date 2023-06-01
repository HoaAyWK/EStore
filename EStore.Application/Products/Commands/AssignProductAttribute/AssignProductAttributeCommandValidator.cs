using FluentValidation;

namespace EStore.Application.Products.Commands.AssignProductAttribute;

public class AssignProductAttributeCommandValidator
    : AbstractValidator<AssignProductAttributeCommand>
{
    public AssignProductAttributeCommandValidator()
    {
        RuleFor(x => x.ProductId).NotNull();

        RuleFor(x => x.ProductAttributeId).NotNull();
    }
}
