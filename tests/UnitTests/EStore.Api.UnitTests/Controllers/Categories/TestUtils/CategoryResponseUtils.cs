using System;
using EStore.Contracts.Categories;

namespace EStore.Api.UnitTests.Controllers.Categories.TestUtils;

public static class CategoryResponseUtils
{
    public static CategoryResponse Create(Guid id, string name, Guid? parentId) =>
        new CategoryResponse(
            Guid.NewGuid(),
            name,
            string.Empty,
            string.Empty,
            parentId,
            DateTime.UtcNow,
            DateTime.UtcNow);
}

