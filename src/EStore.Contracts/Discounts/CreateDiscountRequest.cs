namespace EStore.Contracts.Discounts;

public record CreateDiscountRequest(
    string Name,
    bool UserPercentage,
    decimal DiscountPercentage,
    decimal DiscountAmount,
    DateTime StartDate,
    DateTime EndDate);
