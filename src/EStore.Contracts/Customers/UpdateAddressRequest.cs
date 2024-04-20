namespace EStore.Contracts.Customers;

public record UpdateAddressRequest(
    bool IsDefault,
    string Street,
    string City,
    int StateId,
    string State,
    int CountryId,
    string Country,
    string ZipCode);
