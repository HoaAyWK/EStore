using EStore.Domain.BrandAggregate.ValueObjects;

namespace EStore.Api.UnitTests.TestUtils.Constants;

public static partial class Constants
{
    public static class Brand
    {
        public static readonly BrandId Id = BrandId.CreateUnique();

        public const string Name = "Brand Name";

        public static readonly string InvalidName = new string('a', Domain.BrandAggregate.Brand.MinNameLength - 1);
    }
}
