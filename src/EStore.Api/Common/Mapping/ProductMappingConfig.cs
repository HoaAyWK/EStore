using EStore.Application.Products.Commands.AddProductAttribute;
using EStore.Application.Products.Commands.AddProductAttributeValue;
using EStore.Application.Products.Commands.AddProductImage;
using EStore.Application.Products.Commands.AddVariant;
using EStore.Application.Products.Commands.CreateProduct;
using EStore.Application.Products.Commands.DeleteAttributeValue;
using EStore.Application.Products.Commands.UpdateProduct;
using EStore.Contracts.Products;
using EStore.Domain.BrandAggregate;
using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.Entities;
using EStore.Domain.ProductAggregate.ValueObjects;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class ProductMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProductRequest, CreateProductCommand>();

        config.NewConfig<AddProductImageRequest, AddProductImageCommand>();

        config.NewConfig<UpdateProductRequest, UpdateProductCommand>();

        config.NewConfig<AddProductAttributeValueRequest, AddProductAttributeValueCommand>();

        config.NewConfig<SelectedAttributes, AttributeSelection>()
            .Map(dest => dest.ProductAttributeId, src => ProductAttributeId.Create(src.ProductAttributeId))
            .Map(dest => dest.ProductAttributeValueId, src => ProductAttributeValueId.Create(src.ProductAttributeValueId));

        config.NewConfig<AddProductVariantRequest, AddVariantCommand>();

        config.NewConfig<DeleteAttributeValueRequest, DeleteAttributeValueCommand>()
            .Map(dest => dest.ProductId, src => src.Id)
            .Map(dest => dest.ProductAttributeId, src => src.AttributeId)
            .Map(dest => dest.ProductAttributeValueId, src => src.AttributeValueId);

        config.NewConfig<AddProductAttributeRequest, AddProductAttributeCommand>();

        config.NewConfig<Guid, CategoryId>()
            .MapWith(src => CategoryId.Create(src));

        config.NewConfig<Guid, BrandId>()
            .MapWith(src => BrandId.Create(src));

        config.NewConfig<AverageRating, AverageRatingResponse>();

        config.NewConfig<Brand, BrandResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);

        // config.NewConfig<Category, CategoryResponse>()
        //     .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<ProductImage, ProductImageResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<ProductAttribute, ProductAttributeResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.AttributeValues, src => src.ProductAttributeValues);

        config.NewConfig<ProductAttributeValue, ProductAttributeValueResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(
                dest => dest.CombinedAttributes,
                src => src.CombinedAttributes.AttributesMap.ToDictionary(
                    k => k.Key.Value,
                    v => v.Value.Select(x => x.Value)
                ));

        config.NewConfig<ProductVariant, ProductVariantResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(
                dest => dest.AssignedProductImageIds,
                src => src.AssignedProductImageIds.Split(
                    " ",
                    StringSplitOptions.RemoveEmptyEntries));

        config.NewConfig<Product, ProductResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Attributes, src => src.ProductAttributes)
            .Map(dest => dest.Variants, src => src.ProductVariants);
    }
}
