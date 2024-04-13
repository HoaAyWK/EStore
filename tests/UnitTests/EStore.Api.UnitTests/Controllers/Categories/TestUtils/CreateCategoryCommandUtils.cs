using EStore.Application.Categories.Commands.CreateCategory;
using EStore.Domain.CategoryAggregate.ValueObjects;

namespace EStore.Api.UnitTests.Controllers.Categories.TestUtils;

public static class CreateCategoryCommandUtils
{
    public static CreateCategoryCommand CreateCommand(
        string name,
        string slug,
        string? imageUrl,
        CategoryId? parentId)
        => new(
            name,
            slug,
            imageUrl,
            parentId);
}
