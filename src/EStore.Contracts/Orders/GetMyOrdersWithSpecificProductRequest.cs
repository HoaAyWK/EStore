namespace EStore.Contracts.Orders;

public record GetMyOrdersWithSpecificProductRequest(
    Guid ProductId,
    Guid? ProductVariantId);
