namespace EStore.Domain.UnitTests.Brands.TestUtils;

public class ValidBrandNameData : TheoryData<string>
{
    public ValidBrandNameData()
    {
        Add(Constants.Brand.Name);
        Add(Constants.Brand.NameForUpdating);
        Add(Constants.Brand.NameHasMinLength);
        Add(Constants.Brand.NameHasMaxLength);
    }
}
