namespace EStore.Contracts.Customers;

public record UpdateAddressRequest(
    string ReceiverName,
    string PhoneNumber,
    bool IsDefault,
    string Street,
    string City,
    int StateId,
    string State,
    int CountryId,
    string Country,
    string ZipCode);
