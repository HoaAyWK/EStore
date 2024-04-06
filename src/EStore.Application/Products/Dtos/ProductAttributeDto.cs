namespace EStore.Application.Products.Dtos;

public class ProductAttributeDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool CanCombine { get; set; }

    public int DisplayOrder { get; set; }

    public IEnumerable<ProductAttributeValueDto> AttributeValues { get; set; } = null!;
}
