using EStore.Domain.BrandAggregate;
using FluentValidation;

namespace EStore.Application.Brands.Commands.CreateBrand;

public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .MinimumLength(Brand.MinNameLength)
            .MaximumLength(Brand.MaxNameLength);
    }
}
