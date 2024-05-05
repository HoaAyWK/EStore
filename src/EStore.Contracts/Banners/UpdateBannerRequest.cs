namespace EStore.Contracts.Banners;

public record UpdateBannerRequest(
    Guid BannerId,
    string Direction,
    int DisplayOrder,
    bool IsActive);
