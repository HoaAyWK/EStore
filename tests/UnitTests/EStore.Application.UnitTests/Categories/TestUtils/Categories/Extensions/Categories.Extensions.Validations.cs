using EStore.Application.Categories.Commands.CreateCategory;
using EStore.Domain.CategoryAggregate;

namespace EStore.Application.UnitTests.Categories.TestUtils.Categories.Extensions;

public static partial class CategoryExtensions
{
    public static void ValidateCreatedFrom(this Category Category, CreateCategoryCommand command)
    {
        Category.Name.Should().Be(command.Name);
        Category.ParentId.Should().Be(command.ParentId);
    }
}