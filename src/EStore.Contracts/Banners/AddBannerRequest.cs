namespace EStore.Contracts.Banners;

public record AddBannerRequest(
    Guid ProductId,
    Guid? ProductVariantId,
    string Direction,
    int DisplayOrder,
    bool IsActive);
