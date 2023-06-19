using EStore.Application.Categories.Commands.DeleteCategory;

namespace EStore.Api.UnitTests.Controllers.Categories.TestUtils;

public static class DeleteCategoryCommandUtils
{
    public static DeleteCategoryCommand Create()
        => new DeleteCategoryCommand(Constants.Category.Id);
}
