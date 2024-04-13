using EStore.Domain.BrandAggregate.ValueObjects;

namespace EStore.Application.UnitTests.TestUtils.Constants;

public static partial class Constants
{
    public static class Brand
    {
        public static readonly BrandId Id = BrandId.CreateUnique();
        
        public const string Name = "Brand Name";

        public const string NameForUpdating = "Brand Name Updated";

        public static readonly string NameHasMinLength = new string ('a', Domain.BrandAggregate.Brand.MinNameLength);

        public static readonly string NameHasMaxLength = new string('a', Domain.BrandAggregate.Brand.MaxNameLength);

        public static readonly string NameUnderMinLength = new string('a', Domain.BrandAggregate.Brand.MinNameLength - 1);

        public static readonly string NameExceedMaxLength = new string('a', Domain.BrandAggregate.Brand.MaxNameLength + 1);

        public static readonly Domain.BrandAggregate.Brand MockExistingBrand =
            Domain.BrandAggregate.Brand.Create(Name, string.Empty).Value;
    }
}
