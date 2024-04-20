using ErrorOr;
using EStore.Domain.Common.Models;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.Common.Errors;

namespace EStore.Domain.CustomerAggregate.Entities;

public class Address : Entity<AddressId>
{
    public const int MinStreetLength = 2;

    public const int MaxStreetLength = 100;

    public const int MinCityLength = 1;

    public const int MaxCityLength = 50;

    public const int MinStateLength = 1;

    public const int MaxStateLength = 50;

    public const int MinCountryLength = 1;

    public const int MaxCountryLength = 50;

    public const int MinZipCodeLength = 1;

    public const int MaxZipCodeLength = 20;

    public bool IsDefault { get; private set; }

    public string Street { get; private set; } = default!;

    public string City { get; private set; } = default!;

    public int StateId { get; private set; }

    public string State { get; private set; } = default!;

    public int CountryId { get; private set; }

    public string Country { get; private set; } = default!;

    public string ZipCode { get; private set; } = default!;

    private Address(
        AddressId id,
        bool isDefault,
        string street,
        string city,
        int stateId,
        string state,
        int countryId,
        string country,
        string zipCode)
        : base(id)
    {
        IsDefault = isDefault;
        Street = street;
        City = city;
        StateId = stateId;
        State = state;
        CountryId = countryId;
        Country = country;
        ZipCode = zipCode;
    }

    public void SetNormal()
    {
        IsDefault = false;
    }

    public static ErrorOr<Address> Create(
        bool isDefault,
        string street,
        string city,
        int stateId,
        string state,
        int countryId,
        string country,
        string zipCode)
    {
        var errors = ValidateAddress(
            street,
            city,
            state,
            country,
            zipCode);

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Address(
            AddressId.CreateUnique(),
            isDefault,
            street,
            city,
            stateId,
            state,
            countryId,
            country,
            zipCode);
    }

    public ErrorOr<Updated> Update(
        bool isDefault,
        string street,
        string city,
        int stateId,
        string state,
        int countryId,
        string country,
        string zipCode)
    {
        var errors = ValidateAddress(
            street,
            city,
            state,
            country,
            zipCode);

        if (errors.Count > 0)
        {
            return errors;
        }

        IsDefault = isDefault;
        Street = street;
        City = city;
        StateId = stateId;
        State = state;
        CountryId = countryId;
        Country = country;
        ZipCode = zipCode;

        return Result.Updated;
    }

    private static List<Error> ValidateAddress(
        string street,
        string city,
        string state,
        string country,
        string zipCode)
    {
        var errors = new List<Error>();

        if (street.Length < MinStreetLength || street.Length > MaxStreetLength)
        {
            errors.Add(Errors.Customer.InvalidStreetLength);
        }

        if (city.Length < MinCityLength || city.Length > MaxCityLength)
        {
            errors.Add(Errors.Customer.InvalidCityLength);
        }

        if (state.Length < MinStateLength || state.Length > MaxStateLength)
        {
            errors.Add(Errors.Customer.InvalidStateLength);
        }

        if (country.Length < MinCountryLength || country.Length > MaxCountryLength)
        {
            errors.Add(Errors.Customer.InvalidCountryLength);
        }

        if (zipCode.Length < MinZipCodeLength || zipCode.Length > MaxZipCodeLength)
        {
            errors.Add(Errors.Customer.InvalidZipCodeLength);
        }

        return errors;
    }
}
