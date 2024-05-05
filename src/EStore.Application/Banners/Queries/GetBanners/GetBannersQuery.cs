using EStore.Contracts.Banners;
using EStore.Contracts.Common;
using MediatR;

namespace EStore.Application.Banners.Queries.GetBanners;

public record GetBannersQuery(
    int Page,
    int PageSize,
    string? Order,
    string? OrderBy)
    : IRequest<PagedList<BannerResponse>>;
