namespace EStore.Domain.UnitTests.Brands.TestUtils;

public class InvalidBrandNameData : TheoryData<string>
{
    public InvalidBrandNameData()
    {
        Add(Constants.Brand.NameUnderMinLength);
        Add(Constants.Brand.NameExceedMaxLength);
    }
}
