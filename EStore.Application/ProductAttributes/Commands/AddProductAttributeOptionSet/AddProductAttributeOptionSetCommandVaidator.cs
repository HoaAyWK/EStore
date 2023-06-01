using FluentValidation;

namespace EStore.Application.ProductAttributes.Commands.AddProductAttributeOptionSet;

public class AddProductAttributeOptionSetCommandValidator : AbstractValidator<AddProductAttributeOptionSetCommand>
{
    public AddProductAttributeOptionSetCommandValidator()
    {
        RuleFor(x => x.ProductAttributeId).NotNull();

        RuleFor(x => x.Name).NotEmpty();
    }
}
