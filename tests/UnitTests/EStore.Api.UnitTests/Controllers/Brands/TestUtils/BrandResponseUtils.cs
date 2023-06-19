using System;
using EStore.Contracts.Brands;

namespace EStore.Api.UnitTests.Controllers.Brands.TestUtils;

public static class BrandResponseUtils
{
    public static BrandResponse Create(string name)
        => new BrandResponse(
            Guid.NewGuid(),
            name,
            DateTime.UtcNow,
            DateTime.UtcNow);
}
