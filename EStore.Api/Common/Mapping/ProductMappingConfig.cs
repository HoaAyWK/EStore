using EStore.Application.Products.Commands.AddProductImage;
using EStore.Application.Products.Commands.CreateProduct;
using EStore.Application.Products.Commands.UpdateProduct;
using EStore.Application.Products.Common;
using EStore.Contracts.Products;
using EStore.Domain.Catalog.ProductAggregate.ValueObjects;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class ProductMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ProductImageRequest, CreateProductImageRequest>();
        
        config.NewConfig<CreateProductRequest, CreateProductCommand>();

        config.NewConfig<AddProductImageRequest, AddProductImageCommand>();

        config.NewConfig<UpdateProductRequest, UpdateProductCommand>();
    }
}
