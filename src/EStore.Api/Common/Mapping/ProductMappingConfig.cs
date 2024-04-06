using System.Text;
using EStore.Application.Products.Commands.AddProductAttribute;
using EStore.Application.Products.Commands.AddProductAttributeValue;
using EStore.Application.Products.Commands.AddProductImage;
using EStore.Application.Products.Commands.AddProductVariant;
using EStore.Application.Products.Commands.CreateProduct;
using EStore.Application.Products.Commands.DeleteAttributeValue;
using EStore.Application.Products.Commands.UpdateProduct;
using EStore.Application.Products.Commands.UpdateProductVariant;
using EStore.Application.Products.Dtos;
using EStore.Contracts.Common;
using EStore.Contracts.Products;
using EStore.Domain.BrandAggregate;
using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.DiscountAggregate.ValueObjects;
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

        config.NewConfig<(Guid, UpdateProductRequest), UpdateProductCommand>()
            .Map(dest => dest.Id, src => ProductId.Create(src.Item1))
            .Map(dest => dest.CategoryId, src => CategoryId.Create(src.Item2.CategoryId))
            .Map(dest => dest.BrandId, src => BrandId.Create(src.Item2.BrandId))
            .Map(
                dest => dest.DiscountId,
                src => src.Item2.DiscountId == null
                    ? null
                    : DiscountId.Create(src.Item2.DiscountId.Value))
            .Map(dest => dest, src => src.Item2);

        config.NewConfig<AddProductAttributeValueRequest, AddProductAttributeValueCommand>();

        config.NewConfig<DeleteAttributeValueRequest, DeleteAttributeValueCommand>()
            .Map(dest => dest.ProductId, src => ProductId.Create(src.Id))
            .Map(dest => dest.ProductAttributeId, src => ProductAttributeId.Create(src.AttributeId))
            .Map(dest => dest.ProductAttributeValueId, src => ProductAttributeValueId.Create(src.AttributeValueId));

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
                    StringSplitOptions.RemoveEmptyEntries))
            .Map(
                dest => dest.AttributeSelection,
                src => src.AttributeSelection.AttributesMap.ToDictionary(
                    k => k.Key.Value,
                    v => v.Value.First().Value))
            .Map(
                dest => dest.Attributes,
                src => src.Attributes.ToDictionary(
                    k => k.Key,
                    v => v.Value));

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

        config.NewConfig<CreateProductVariantRequest.SelectedAttribute, SelectedAttribute>()
            .Map(
                dest => dest.ProductAttributeId,
                src => ProductAttributeId.Create(src.ProductAttributeId))
            .Map(
                dest => dest.ProductAttributeValueId,
                src => ProductAttributeValueId.Create(src.ProductAttributeValueId));

        config.NewConfig<(Guid, CreateProductVariantRequest), AddProductVariantCommand>()
            .Map(dest => dest.ProductId, src => ProductId.Create(src.Item1))
            .Map(dest => dest.SelectedAttributes, src => src.Item2.SelectedAttributes)
            .Map(
                dest => dest.AssignedProductImageIds,
                src => src.Item2.AssignedImageIds)
            .Map(dest => dest, src => src.Item2);

        config.NewConfig<(Guid, Guid, UpdateProductVariantRequest), UpdateProductVariantCommand>()
            .Map(dest => dest.ProductId, src => ProductId.Create(src.Item1))
            .Map(dest => dest.ProductVariantId, src => ProductVariantId.Create(src.Item2))
            .Map(
                dest => dest.ImageIds,
                src => src.Item3.AssignedImageIds)
            .Map(dest => dest, src => src.Item3);
    }
}
