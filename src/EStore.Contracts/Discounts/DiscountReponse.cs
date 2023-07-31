namespace EStore.Contracts.Discounts;

public record DiscountResponse(
    Guid Id,
    string Name,
    bool UsePercentage,
    decimal DiscountPercentage,
    decimal DiscountAmount,
    DateTime StartDate,
    DateTime EndDate);
