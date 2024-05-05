namespace EStore.Contracts.Orders;

public record GetOrdersRequest(
    int Page = 1,
    int PageSize = 10,
    string OrderStatus = "All", 
    string? Order = null,
    string? OrderBy = null,
    int? OrderNumber = null);
