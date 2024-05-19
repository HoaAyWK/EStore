namespace EStore.Contracts.Searching;

public record RebuildProductVariantRequest(
    Guid ProductId,
    Guid ProductVariantId);
