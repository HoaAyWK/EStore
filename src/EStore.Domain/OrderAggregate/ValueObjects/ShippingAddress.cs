using EStore.Domain.Common.Models;

namespace EStore.Domain.OrderAggregate.ValueObjects;

public sealed class ShippingAddress : ValueObject
{
    public string ReceiverName { get; } = string.Empty;

    public string PhoneNumber { get; } = string.Empty;

    public string Street { get; } = string.Empty;

    public string City { get; } = string.Empty;

    public string State { get; } = string.Empty;

    public string Country { get; } = string.Empty;

    public string ZipCode { get; } = string.Empty;

    private ShippingAddress()
    {
    }

    private ShippingAddress(
        string receiverName,
        string phoneNumber,
        string street,
        string city,
        string state,
        string country,
        string zipCode)
    {
        ReceiverName = receiverName;
        PhoneNumber = phoneNumber;
        Street = street;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipCode;
    }

    public static ShippingAddress Create(
        string receiverName,
        string phoneNumber,
        string street,
        string city,
        string state,
        string country,
        string zipCode)
    {
        return new(
            receiverName,
            phoneNumber,
            street,
            city,
            state,
            country,
            zipCode);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return ReceiverName;
        yield return PhoneNumber;
        yield return Street;
        yield return City;
        yield return State;
        yield return Country;
        yield return ZipCode;
    }
}
