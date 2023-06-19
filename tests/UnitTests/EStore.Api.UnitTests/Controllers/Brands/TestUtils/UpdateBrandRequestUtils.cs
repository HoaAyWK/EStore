using EStore.Contracts.Brands;

namespace EStore.Api.UnitTests.Controllers.Brands.TestUtils;

public static class UpdateBrandRequestUtils
{
    
    public static UpdateBrandRequest Create()
        => new UpdateBrandRequest(Constants.Brand.Name);
}
