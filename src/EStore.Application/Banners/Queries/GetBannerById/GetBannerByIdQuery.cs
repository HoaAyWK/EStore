using ErrorOr;
using EStore.Contracts.Banners;
using EStore.Domain.BannerAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Banners.Queries.GetBannerById;

public record GetBannerByIdQuery(BannerId Id)
    : IRequest<ErrorOr<BannerResponse>>;
