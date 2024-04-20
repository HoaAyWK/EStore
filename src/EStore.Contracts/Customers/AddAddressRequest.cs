namespace EStore.Contracts.Customers;

public record AddAddressRequest(
    Guid CustomerId,
    bool IsDefault,
    string Street,
    string City,
    int StateId,
    string State,
    int CountryId,
    string Country,
    string ZipCode);
