namespace EStore.Contracts.Banners;

public class BannerResponse
{
    public Guid Id { get; set; }

    public ProductResponse? Product { get; set; }

    public string Direction { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }
    
    public bool IsActive { get; set; }

    public DateTime CreatedDateTime { get; set; }
}

public class ProductResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ShortDescription { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public ProductVariantResponse? ProductVariant { get; set; }
}

public record ProductVariantResponse(
    Guid Id,
    Dictionary<string, string> Attributes);
