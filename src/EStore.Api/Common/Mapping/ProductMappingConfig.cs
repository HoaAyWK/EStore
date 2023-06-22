using EStore.Application.Products.Commands.AddProductAttribute;
using EStore.Application.Products.Commands.AddProductAttributeValue;
using EStore.Application.Products.Commands.AddProductImage;
using EStore.Application.Products.Commands.CreateProduct;
using EStore.Application.Products.Commands.DeleteAttributeValue;
using EStore.Application.Products.Commands.UpdateProduct;
using EStore.Application.Products.Dtos;
using EStore.Contracts.Common;
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
    public ProductMappingConfig()
    {
    }

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProductRequest, CreateProductCommand>();

        config.NewConfig<AddProductImageRequest, AddProductImageCommand>();

        config.NewConfig<UpdateProductRequest, UpdateProductCommand>();

        config.NewConfig<AddProductAttributeValueRequest, AddProductAttributeValueCommand>();

        // config.NewConfig<SelectedAttributes, AttributeSelection>()
        //     .Map(dest => dest.ProductAttributeId, src => ProductAttributeId.Create(src.ProductAttributeId))
        //     .Map(dest => dest.ProductAttributeValueId, src => ProductAttributeValueId.Create(src.ProductAttributeValueId));

        // config.NewConfig<AddProductVariantRequest, AddVariantCommand>();

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

        config.NewConfig<BrandDto, BrandResponse>();

        config.NewConfig<CategoryDto, CategoryDto>();

        config.NewConfig<ProductImageDto, ProductImageResponse>();

        config.NewConfig<ProductAttributeDto, ProductAttributeResponse>()
            .Map(dest => dest.AttributeValues, src => src.AttributeValues);

        config.NewConfig<ProductAttributeValueDto, ProductAttributeValueResponse>()
            .Map(
                dest => dest.CombinedAttributes,
                src => src.AttributeSelection.AttributesMap.ToDictionary(
                    k => k.Key.Value,
                    v => v.Value.Select(x => x.Value)
                ));

        config.NewConfig<ProductVariantDto, ProductVariantResponse>()
            .Map(
                dest => dest.AssignedProductImageIds,
                src => src.AssignedProductImageIds.Split(
                    " ",
                    StringSplitOptions.RemoveEmptyEntries));

        config.NewConfig<Brand, BrandResponse>()
            .Map(src => src.Id, dest => dest.Id.Value);

        config.NewConfig<Category, CategoryDto>()
            .Map(src => src.Id, dest => dest.Id.Value);

        config.NewConfig<ProductImage, ProductImageResponse>()
            .Map(src => src.Id, dest => dest.Id.Value);

        config.NewConfig<ProductAttributeValue, ProductAttributeValueResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<ProductDto, ProductResponse>();

        config.NewConfig<Product, ProductResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<PagedList<ProductDto>, PagedList<ProductResponse>>();
    }
}
