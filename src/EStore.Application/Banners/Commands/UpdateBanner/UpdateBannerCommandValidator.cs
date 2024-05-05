using EStore.Domain.BannerAggregate.Enumerations;
using FluentValidation;

namespace EStore.Application.Banners.Commands.UpdateBanner;

public class UpdateBannerCommandValidator : AbstractValidator<UpdateBannerCommand>
{
    public UpdateBannerCommandValidator()
    {
        RuleFor(x => x.BannerId)
            .NotEmpty();

        RuleFor(x => x.Direction)
            .NotEmpty()
            .Must(BeAValidDirection);

        RuleFor(x => x.DisplayOrder)
            .GreaterThan(0);
    }


    private static bool BeAValidDirection(string direction)
        => BannerDirection.FromName(direction) is not null;
}
