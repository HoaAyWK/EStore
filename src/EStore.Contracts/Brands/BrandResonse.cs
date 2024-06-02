namespace EStore.Contracts.Brands;

public record BrandResponse(
    Guid Id,
    string Name,
    string? ImageUrl,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime);
