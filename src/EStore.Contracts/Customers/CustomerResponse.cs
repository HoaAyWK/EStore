namespace EStore.Contracts.Customers;

public record CustomerResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? AvatarUrl,
    string? Role,
    bool? EmailConfirmed);
