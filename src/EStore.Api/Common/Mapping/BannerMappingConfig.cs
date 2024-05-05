using EStore.Application.Banners.Commands.AddBanner;
using EStore.Application.Banners.Commands.UpdateBanner;
using EStore.Application.Banners.Queries.GetBannerById;
using EStore.Application.Banners.Queries.GetBanners;
using EStore.Contracts.Banners;
using EStore.Domain.BannerAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class BannerMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddBannerRequest, AddBannerCommand>()
            .Map(dest => dest.ProductId, src => ProductId.Create(src.ProductId))
            .Map(
                dest => dest.ProductVariantId,
                src => src.ProductVariantId != null
                    ? ProductVariantId.Create(src.ProductVariantId.Value)
                    : null);

        config.NewConfig<UpdateBannerRequest, UpdateBannerCommand>()
            .Map(dest => dest.BannerId, src => BannerId.Create(src.BannerId));

        config.NewConfig<Guid, BannerId>()
            .Map(dest => dest, src => BannerId.Create(src));

        config.NewConfig<Guid, GetBannerByIdQuery>()
            .Map(dest => dest.Id, src => BannerId.Create(src));

        config.NewConfig<GetBannersRequest, GetBannersQuery>();
    }
}
