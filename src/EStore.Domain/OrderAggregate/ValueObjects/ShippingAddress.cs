using EStore.Domain.Common.Models;

namespace EStore.Domain.OrderAggregate.ValueObjects;

public sealed class ShippingAddress : ValueObject
{
    public string Street { get; } = null!;

    public string City { get; } = null!;

    public string State { get; } = null!;

    public string Country { get; } = null!;

    public string ZipCode { get; } = null!;

    private ShippingAddress()
    {
    }

    private ShippingAddress(
        string street,
        string city,
        string state,
        string country,
        string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipCode;
    }

    public static ShippingAddress Create(
        string street,
        string city,
        string state,
        string country,
        string zipCode)
    {
        return new(
            street,
            city,
            state,
            country,
            zipCode);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return Country;
        yield return ZipCode;
    }
}
