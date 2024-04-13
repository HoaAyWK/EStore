using EStore.Contracts.Brands;

namespace EStore.Api.UnitTests.Controllers.Brands.TestUtils;

public static class CreateBrandRequestUtils
{
    public static CreateBrandRequest Create(string name)
        => new(name, string.Empty);
}
