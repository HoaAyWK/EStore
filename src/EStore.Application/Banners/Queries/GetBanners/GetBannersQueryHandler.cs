using EStore.Application.Banners.Services;
using EStore.Contracts.Banners;
using EStore.Contracts.Common;
using MediatR;

namespace EStore.Application.Banners.Queries.GetBanners;

public class GetBannersQueryHandler
    : IRequestHandler<GetBannersQuery, PagedList<BannerResponse>>
{
    private readonly IBannerReadService _bannerReadService;

    public GetBannersQueryHandler(IBannerReadService bannerReadService)
    {
        _bannerReadService = bannerReadService;
    }

    public async Task<PagedList<BannerResponse>> Handle(
        GetBannersQuery request,
        CancellationToken cancellationToken)
    {
        return await _bannerReadService.GetListPagedAsync(
            request.Page,
            request.PageSize,
            request.Order,
            request.OrderBy);
    }
}
