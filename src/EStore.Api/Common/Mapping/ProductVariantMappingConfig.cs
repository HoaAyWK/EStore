using EStore.Application.ProductVariants.Commands.CreateProductVariant;
using EStore.Application.ProductVariants.Commands.UpdateProductVariant;
using EStore.Contracts.ProductVariants;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.ProductVariantAggregate.ValueObjects;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class ProductVariantMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProductVariantRequest.SelectedAttribute, SelectedAttribute>()
            .Map(dest => dest.ProductAttributeId, src => ProductAttributeId.Create(src.ProductAttributeId))
            .Map(dest => dest.ProductAttributeValueId, src => ProductAttributeValueId.Create(src.ProductAttributeValueId));


        config.NewConfig<CreateProductVariantRequest, CreateProductVariantCommand>()
            .Map(dest => dest.ProductId, src => ProductId.Create(src.ProductId))
            .Map(
                dest => dest.AssignedProductImageIds,
                src => src.AssignedImageIds);

        config.NewConfig<(Guid, UpdateProductVariantRequest), UpdateProductVariantCommand>()
            .Map(dest => dest.Id, src => ProductVariantId.Create(src.Item1))
            .Map(
                dest => dest.AssignedImageIds,
                src => src.Item2.AssignedImageIds)
            .Map(dest => dest, src => src.Item2);
    }
}

