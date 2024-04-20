namespace EStore.Contracts.Customers;

public record UpdateCustomerRequest(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? AvatarUrl);
