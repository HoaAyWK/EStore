using EStore.Domain.BrandAggregate;
using FluentValidation;

namespace EStore.Application.Brands.Commands.UpdateBrand;

public class UpdateBrandCommandValidator
    : AbstractValidator<UpdateBrandCommand>
{
    public UpdateBrandCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name).NotEmpty()
            .MinimumLength(Brand.MinNameLength)
            .MaximumLength(Brand.MaxNameLength);
    }
}
