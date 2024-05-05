using ErrorOr;
using EStore.Application.Banners.Services;
using EStore.Contracts.Banners;
using MediatR;

namespace EStore.Application.Banners.Queries.GetBannerById;

public class GetBannerByIdQueryHandler
    : IRequestHandler<GetBannerByIdQuery, ErrorOr<BannerResponse>>
{
    private readonly IBannerReadService _bannerReadService;

    public GetBannerByIdQueryHandler(IBannerReadService bannerReadService)
    {
        _bannerReadService = bannerReadService;
    }

    public async Task<ErrorOr<BannerResponse>> Handle(
        GetBannerByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _bannerReadService.GetByIdAsync(request.Id);
    }
}
