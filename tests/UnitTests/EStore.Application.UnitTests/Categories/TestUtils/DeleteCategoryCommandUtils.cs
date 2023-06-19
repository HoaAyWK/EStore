using EStore.Application.Categories.Commands.DeleteCategory;
using EStore.Domain.CategoryAggregate.ValueObjects;

namespace EStore.Application.UnitTests.Categories.TestUtils;

public static class DeleteCategoryCommandUtils
{
    public static DeleteCategoryCommand CreateCommand(CategoryId id)
        => new DeleteCategoryCommand(id);
}

