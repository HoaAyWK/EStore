using EStore.Application.Products.Commands.CreateProduct;
using EStore.Application.UnitTests.TestUtils.Constants;
using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.CategoryAggregate.ValueObjects;

namespace EStore.Application.UnitTests.Products.Commands.TestUtils;

public static class CreateProductCommandUtils
{
    public static CreateProductCommand CreateCommand(
        string? name = null,
        string? description = null,
        bool published = true,
        decimal price = 0,
        int displayOrder = 0,
        BrandId? brandId = null,
        CategoryId? categoryId = null,
        bool hasVariant = false)
    {
        return new CreateProductCommand(
            name ?? Constants.Product.Name,
            description ?? Constants.Product.Description,
            published,
            price,
            displayOrder,
            brandId ?? Constants.Product.BrandId,
            categoryId ?? Constants.Product.CategoryId,
            hasVariant);
    }
}
