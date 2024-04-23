namespace EStore.Contracts.Common;

public record AddressResponse(
    Guid Id,
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
