namespace EStore.Contracts.Products;

public record DeleteAttributeValueRequest(
    Guid Id,
    Guid AttributeId,
    Guid AttributeValueId);
