namespace EStore.Contracts.Brands;

public record BrandResponse(
    Guid Id,
    string Name,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime);
