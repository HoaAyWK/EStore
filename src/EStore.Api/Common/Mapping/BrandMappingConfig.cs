using EStore.Application.Brands.Commands.CreateBrand;
using EStore.Application.Brands.Commands.DeleteBrand;
using EStore.Application.Brands.Commands.UpdateBrand;
using EStore.Contracts.Brands;
using EStore.Domain.BrandAggregate;
using EStore.Domain.BrandAggregate.ValueObjects;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class BrandMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateBrandRequest, CreateBrandCommand>();

        config.NewConfig<UpdateBrandRequest, UpdateBrandCommand>();
        
        config.NewConfig<Brand, BrandResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<(Guid, UpdateBrandRequest), UpdateBrandCommand>()
            .Map(dest => dest.Id, src => BrandId.Create(src.Item1))
            .Map(dest => dest, src => src.Item2);

        config.NewConfig<Guid, DeleteBrandCommand>()
            .Map(dest => dest, src => BrandId.Create(src));
    }
}
