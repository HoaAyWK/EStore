namespace EStore.Contracts.ProductAttributes;

public record AddProductAttributeOptionSetRequest(
    string ProductAttributeId,
    string Name);
