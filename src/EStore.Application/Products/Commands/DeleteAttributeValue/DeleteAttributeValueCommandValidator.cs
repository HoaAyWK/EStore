using FluentValidation;

namespace EStore.Application.Products.Commands.DeleteAttributeValue;

public class DeleteAttributeValueCommandValidator
    : AbstractValidator<DeleteAttributeValueCommand>
{
    public DeleteAttributeValueCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();

        RuleFor(x => x.ProductAttributeId).NotEmpty();

        RuleFor(x => x.ProductAttributeValueId).NotEmpty();
    }
}
