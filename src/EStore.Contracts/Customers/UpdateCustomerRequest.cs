namespace EStore.Contracts.Customers;

public record UpdateCustomerRequest(
    string FirstName,
    string LastName);
