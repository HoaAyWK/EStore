using ErrorOr;
using EStore.Domain.BannerAggregate;
using EStore.Domain.BannerAggregate.Enumerations;
using EStore.Domain.BannerAggregate.Repositories;
using EStore.Domain.Common.Errors;
using EStore.Domain.ProductAggregate.Entities;
using EStore.Domain.ProductAggregate.Repositories;
using MediatR;

namespace EStore.Application.Banners.Commands.AddBanner;

public class AddBannerCommandHandler
    : IRequestHandler<AddBannerCommand, ErrorOr<Banner>>
{
    private readonly IBannerRepository _bannerRepository;
    private readonly IProductRepository _productRepository;

    public AddBannerCommandHandler(
        IBannerRepository bannerRepository,
        IProductRepository productRepository)
    {
        _bannerRepository = bannerRepository;
        _productRepository = productRepository;
    }

    public async Task<ErrorOr<Banner>> Handle(
        AddBannerCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        ProductVariant? productVariant = null;

        if (request.ProductVariantId is not null)
        {
            productVariant = product.ProductVariants
                .Where(variant => variant.Id == request.ProductVariantId)
                .SingleOrDefault();

            if (productVariant is null)
            {
                return Errors.Product.ProductVariantNotFound;
            }
        }

        var bannerDirection = BannerDirection.FromName(request.Direction);
        var banner = Banner.Create(
            request.ProductId,
            request.ProductVariantId,
            bannerDirection!,
            request.DisplayOrder,
            request.IsActive);

        await _bannerRepository.AddAsync(banner, cancellationToken);

        return banner;
    }
}
