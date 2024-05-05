using ErrorOr;
using EStore.Contracts.Banners;
using EStore.Contracts.Common;
using EStore.Domain.BannerAggregate.ValueObjects;

namespace EStore.Application.Banners.Services;

public interface IBannerReadService
{
    Task<ErrorOr<BannerResponse>> GetByIdAsync(BannerId id);
    Task<PagedList<BannerResponse>> GetListPagedAsync(
        int page,
        int pageSize,
        string? order,
        string? orderBy);
}
