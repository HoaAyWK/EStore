namespace EStore.Contracts.Common;

public record AddressResponse(
    Guid Id,
    bool IsDefault,
    string Street,
    string City,
    int StateId,
    string State,
    int CountryId,
    string Country,
    string ZipCode);
