using ErrorOr;
using EStore.Domain.BannerAggregate;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Banners.Commands.AddBanner;

public record AddBannerCommand(
    ProductId ProductId,
    ProductVariantId? ProductVariantId,
    string Direction,
    int DisplayOrder,
    bool IsActive)
    : IRequest<ErrorOr<Banner>>;
