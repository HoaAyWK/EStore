using FluentValidation;

namespace EStore.Application.Products.Commands.UpdateProductAttributeValue;

public class UpdateProductAttributeValueCommandValidator
    : AbstractValidator<UpdateProductAttributeValueCommand>
{
    public UpdateProductAttributeValueCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);
    }
}
