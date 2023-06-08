using FluentValidation;

namespace EStore.Application.Brands.Commands.UpdateBrand;

public class UpdateBrandCommandValidator
    : AbstractValidator<UpdateBrandCommand>
{
    public UpdateBrandCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name).NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);
    }
}
