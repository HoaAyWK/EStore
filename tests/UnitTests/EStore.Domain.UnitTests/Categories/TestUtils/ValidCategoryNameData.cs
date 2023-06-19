namespace EStore.Domain.UnitTests.Categories.TestUtils;

public class ValidCategoryNameData : TheoryData<string>
{
    public ValidCategoryNameData()
    {
        Add(Constants.Category.Name);
        Add(Constants.Category.NameHasMinLength);
        Add(Constants.Category.NameHasMaxLength);
    }
}
