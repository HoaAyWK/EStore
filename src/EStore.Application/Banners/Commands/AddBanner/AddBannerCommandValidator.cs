using EStore.Domain.BannerAggregate.Enumerations;
using FluentValidation;

namespace EStore.Application.Banners.Commands.AddBanner;

public class AddBannerCommandValidator : AbstractValidator<AddBannerCommand>
{
    public AddBannerCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();
        RuleFor(x => x.Direction)
            .NotEmpty()
            .Must(BeAValidDirection);

        RuleFor(x => x.DisplayOrder)
            .NotEmpty();
    }

    private static bool BeAValidDirection(string direction)
        => BannerDirection.FromName(direction) is not null;
}
