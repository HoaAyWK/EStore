using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Application.UnitTests.TestUtils.Constants;

public static partial class Constants
{
    public static class Product
    {
        public static readonly ProductId Id = ProductId.CreateUnique();

        public const string Name = "Product Name";

        public static readonly string NameHasMinLength = new string ('a', Domain.ProductAggregate.Product.MinNameLength);

        public static readonly string NameHasMaxLength = new string('a', Domain.ProductAggregate.Product.MaxNameLength);

        public static readonly string NameUnderMinLength = new string('a', Domain.ProductAggregate.Product.MinNameLength - 1);

        public static readonly string NameExceedMaxLength = new string('a', Domain.ProductAggregate.Product.MaxNameLength + 1);

        public const string Description = "Product Description";

        public const bool Published = true;

        public static readonly BrandId BrandId = BrandId.CreateUnique();

        public static readonly CategoryId CategoryId = CategoryId.CreateUnique();
    }
}
