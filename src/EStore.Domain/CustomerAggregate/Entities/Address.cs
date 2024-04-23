using ErrorOr;
using EStore.Domain.Common.Models;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.Common.Errors;

namespace EStore.Domain.CustomerAggregate.Entities;

public class Address : Entity<AddressId>
{
    public const int MinReceiverNameLength = 2;

    public const int MaxReceiverNameLength = 100;

    public const int MinPhoneNumberLength = 7;

    public const int MaxPhoneNumberLength = 15;

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

    public string ReceiverName { get; private set; } = default!;

    public string PhoneNumber { get; private set; } = default!;

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
        string receiverName,
        string phoneNumber,
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
        ReceiverName = receiverName;
        PhoneNumber = phoneNumber;
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
        string receiverName,
        string phoneNumber,
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
            receiverName,
            phoneNumber,
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
            receiverName,
            phoneNumber,
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
        string receiverName,
        string phoneNumber,
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
            receiverName,
            phoneNumber,
            street,
            city,
            state,
            country,
            zipCode);

        if (errors.Count > 0)
        {
            return errors;
        }

        ReceiverName = receiverName;
        PhoneNumber = phoneNumber;
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
        string receiverName,
        string phoneNumber,
        string street,
        string city,
        string state,
        string country,
        string zipCode)
    {
        var errors = new List<Error>();

        if (receiverName.Length < MinReceiverNameLength || receiverName.Length > MaxReceiverNameLength)
        {
            errors.Add(Errors.Customer.InvalidReceiverNameLength);
        }

        if (phoneNumber.Length < MinPhoneNumberLength || phoneNumber.Length > MaxPhoneNumberLength)
        {
            errors.Add(Errors.Customer.InvalidPhoneNumberLength);
        }

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
