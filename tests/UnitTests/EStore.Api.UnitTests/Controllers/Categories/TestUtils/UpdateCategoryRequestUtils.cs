using EStore.Contracts.Categories;

namespace EStore.Api.UnitTests.Controllers.Categories.TestUtils;

public static class UpdateCategoryRequestUtils
{
    public static UpdateCategoryRequest Create(string name, string? parentId)
        => new UpdateCategoryRequest(name, parentId);
}
