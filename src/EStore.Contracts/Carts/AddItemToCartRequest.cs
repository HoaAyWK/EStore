namespace EStore.Contracts.Carts;

public record AddItemToCartRequest(
    Guid ProductId,
    Guid? ProductVariantId);
