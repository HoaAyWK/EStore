namespace EStore.Contracts.Discounts;

public record UpdateDiscountRequest(
    string Name,
    bool UsePercentage,
    decimal DiscountPercentage,
    decimal DiscountAmount,
    DateTime StartDate,
    DateTime EndDate);
