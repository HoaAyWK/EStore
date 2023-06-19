using EStore.Application.Categories.Commands.UpdateCategory;
using EStore.Application.UnitTests.TestUtils.Constants;
using EStore.Domain.CategoryAggregate.ValueObjects;

namespace EStore.Application.UnitTests.Categories.TestUtils;

public static class UpdateCategoryCommandUtils
{
    public static UpdateCategoryCommand CreateCommand(
        CategoryId? id = null,
        string? name = null,
        CategoryId? parentId = null)
    {
        return new UpdateCategoryCommand(
            id ?? Constants.Category.Id,
            name ?? Constants.Category.Name,
            parentId);
    }
}

