using ErrorOr;
using EStore.Domain.Common.Models;
using Newtonsoft.Json;
using static EStore.Domain.Common.Errors.Errors;

namespace EStore.Domain.Common.ValueObjects;

public class Address : ValueObject
{
    public const int MinStreetLength = 2;

    public const int MaxStreetLength = 100;

    public const int MinCityLength = 1;

    public const int MaxCityLength = 50;

    public const int MinStateLength = 1;

    public const int MaxStateLength = 50;

    public const int MinCountryLength = 1;

    public const int MaxCountryLength = 50;

    public string Street { get; private set; } = default!;

    public string City { get; private set; } = default!;

    public string State { get; private set; } = default!;

    public string Country { get; private set; } = default!;

    [JsonConstructor]
    private Address(string street, string city, string state, string country)
    {
        Street = street;
        City = city;
        State = state;
        Country = country;
    }

    public static ErrorOr<Address> Create(
        string street,
        string city,
        string state,
        string country)
    {
        if (street.Length < MinStreetLength || street.Length > MaxStreetLength)
        {
            return General.InvalidStreetLength;
        }

        if (city.Length < MinCityLength || city.Length > MaxCityLength)
        {
            return General.InvalidCityLength;
        }

        if (state.Length < MinStateLength || state.Length > MaxStateLength)
        {
            return General.InvalidStateLength;
        }

        if (country.Length < MinCountryLength || country.Length > MaxCountryLength)
        {
            return General.InvalidCountryLength;
        }

        return new Address(street, city, state, country);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return Country;
    }
}
