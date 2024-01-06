namespace EStore.Contracts.Searching;

public record SearchProductsRequest(string? Query, int Page, int PageSize);
