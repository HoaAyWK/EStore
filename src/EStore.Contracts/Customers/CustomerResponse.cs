namespace EStore.Contracts.Customers;

public record CustomerResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email);
