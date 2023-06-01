using EStore.Application.Brands.Commands.CreateBrand;
using EStore.Contracts.Brands;
using EStore.Domain.Catalog.BrandAggregate;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class BrandMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateBrandRequest, CreateBrandCommand>();
        
        config.NewConfig<Brand, BrandResponse>()
            .Map(
                dest => dest.Id,
                src => src.Id.Value);
    }
}
