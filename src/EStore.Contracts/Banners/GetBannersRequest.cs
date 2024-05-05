namespace EStore.Contracts.Banners;

public record GetBannersRequest(
    int Page,
    int PageSize,
    string? Order,
    string? OrderBy);
