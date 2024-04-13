using EStore.Contracts.Categories;

namespace EStore.Api.UnitTests.Controllers.Categories.TestUtils;

public static class CreateCategoryRequestUtils
{
    public static CreateCategoryRequest CreateRequest(string name, string? parentId)
        => new CreateCategoryRequest(
            Constants.Category.Name,
            Constants.Category.Name,
            string.Empty,
            parentId);
}
