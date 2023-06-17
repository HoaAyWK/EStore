using FluentValidation;

namespace EStore.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.Description).NotEmpty();

        RuleFor(x => x.Published).NotNull();

        RuleFor(x => x.Price).NotNull();
    }
}
