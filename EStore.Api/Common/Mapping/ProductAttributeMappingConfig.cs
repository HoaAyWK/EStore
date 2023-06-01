using EStore.Application.ProductAttributes.Commands.AddProductAttributeOption;
using EStore.Application.ProductAttributes.Commands.AddProductAttributeOptionSet;
using EStore.Application.ProductAttributes.Commands.CreateProductAttribute;
using EStore.Application.ProductAttributes.Commands.UpdateProductAttribute;
using EStore.Contracts.ProductAttributes;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class ProductAttributeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProductAttributeRequest, CreateProductAttributeCommand>();

        config.NewConfig<AddProductAttributeOptionSetRequest, AddProductAttributeOptionSetCommand>();

        config.NewConfig<AddProductAttributeOptionRequest, AddProductAttributeOptionCommand>();

        config.NewConfig<UpdateProductAttributeRequest, UpdateProductAttributeCommand>();
    }
}
