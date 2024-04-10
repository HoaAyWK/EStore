namespace EStore.Application.Products.Dtos;

public class ProductReviewCommentDto
{
    public Guid Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedDateTime { get; set; }

    public DateTime UpdatedDateTime { get; set; }

    public ProductReviewOwnerDto? Owner { get; set; }
}
