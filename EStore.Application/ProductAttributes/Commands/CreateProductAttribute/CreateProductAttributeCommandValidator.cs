using FluentValidation;

namespace EStore.Application.ProductAttributes.Commands.CreateProductAttribute;

public class CreateProductAttributeCommandValidator : AbstractValidator<CreateProductAttributeCommand>
{
    public CreateProductAttributeCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
