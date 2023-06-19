using EStore.Application.Products.Commands.CreateProduct;
using EStore.Domain.ProductAggregate;

namespace EStore.Application.UnitTests.Products.Commands.TestUtils.Products.Extensions;

public static class ProductExtensions
{
    public static void ValidateCreatedFrom(this Product product, CreateProductCommand command)
    {
        product.Name.Should().Be(command.Name);
        product.Description.Should().Be(command.Description);
        product.Published.Should().Be(command.Published);
        product.BrandId.Should().Be(command.BrandId);
        product.CategoryId.Should().Be(command.CategoryId);
    }
}
