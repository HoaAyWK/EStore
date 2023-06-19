using EStore.Application.Categories.Commands.UpdateCategory;
using EStore.Domain.CategoryAggregate.ValueObjects;

namespace EStore.Api.UnitTests.Controllers.Categories.TestUtils;

public static class UpdateCategoryCommandUtils
{
    public static UpdateCategoryCommand Create(string name)
        => new UpdateCategoryCommand(
            CategoryId.CreateUnique(),
            name,
            null);
}
