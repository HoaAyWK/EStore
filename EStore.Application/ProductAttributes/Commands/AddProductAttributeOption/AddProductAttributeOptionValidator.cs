using FluentValidation;

namespace EStore.Application.ProductAttributes.Commands.AddProductAttributeOption;

public class AddProductAttributeOptionValidator : AbstractValidator<AddProductAttributeOptionCommand>
{
    public AddProductAttributeOptionValidator()
    {
        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.ProductAttributeId).NotNull();

        RuleFor(x => x.ProductAttributeOptionSetId).NotNull();
    }
}
