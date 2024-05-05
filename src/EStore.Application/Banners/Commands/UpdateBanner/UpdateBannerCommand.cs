using ErrorOr;
using EStore.Domain.BannerAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Banners.Commands.UpdateBanner;

public record UpdateBannerCommand(
    BannerId BannerId,
    string Direction,
    int DisplayOrder,
    bool IsActive)
    : IRequest<ErrorOr<Updated>>;
