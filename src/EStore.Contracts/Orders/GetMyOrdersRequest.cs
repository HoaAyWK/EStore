namespace EStore.Contracts.Orders;

public record GetMyOrdersRequest(
    int Page = 1,
    int PageSize = 5,
    string? Status = null);
    