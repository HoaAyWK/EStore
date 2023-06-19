namespace EStore.Domain.UnitTests.Categories.TestUtils;

public class InvalidCategoryNameData : TheoryData<string>
{
    public InvalidCategoryNameData()
    {
        Add(Constants.Category.NameUnderMinLength);
        Add(Constants.Category.NameExceedMaxLength);
    }
}

