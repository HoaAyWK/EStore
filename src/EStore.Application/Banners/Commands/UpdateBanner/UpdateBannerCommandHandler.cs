using ErrorOr;
using EStore.Domain.BannerAggregate.Enumerations;
using EStore.Domain.BannerAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Banners.Commands.UpdateBanner;

public class UpdateBannerCommandHandler : IRequestHandler<UpdateBannerCommand, ErrorOr<Updated>>
{
    private readonly IBannerRepository _bannerRepository;

    public UpdateBannerCommandHandler(IBannerRepository bannerRepository)
    {
        _bannerRepository = bannerRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdateBannerCommand request,
        CancellationToken cancellationToken)
    {
        var banner = await _bannerRepository.GetByIdAsync(request.BannerId);

        if (banner is null)
        {
            return Errors.Banner.NotFound;
        }

        var direction = BannerDirection.FromName(request.Direction);

        banner.UpdateDetails(
            direction!,
            request.DisplayOrder,
            request.IsActive);

        return Result.Updated;
    }
}
