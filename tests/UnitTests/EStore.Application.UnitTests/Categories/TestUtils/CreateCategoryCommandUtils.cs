using EStore.Application.Categories.Commands.CreateCategory;
using EStore.Application.UnitTests.TestUtils.Constants;
using EStore.Domain.CategoryAggregate.ValueObjects;

namespace EStore.Application.UnitTests.Categories.TestUtils;

public static class CreateCategoryCommandUtils
{
    public static CreateCategoryCommand CreateCommand(string? name = null, CategoryId? parentId = null) =>
        new(name ?? Constants.Category.Name, Constants.Category.Name, string.Empty, parentId);
}
