namespace EStore.Contracts.Carts;

public record RemoveCartItemRequest(Guid Id, Guid ItemId);
