using ErrorOr;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.Common.Errors;
using EStore.Domain.Common.Models;
using EStore.Domain.CustomerAggregate.Entities;
using EStore.Domain.CustomerAggregate.Events;
using EStore.Domain.CustomerAggregate.ValueObjects;

namespace EStore.Domain.CustomerAggregate;

public sealed class Customer : AggregateRoot<CustomerId>, IAuditableEntity, ISoftDeletableEntity
{
    public const int MinFirstNameLength = 1;
    
    public const int MaxFirstNameLength = 100;

    public const int MinLastNameLength = 1;

    public const int MaxLastNameLength = 100;

    public const int MinPhoneNumberLength = 7;

    public const int MaxPhoneNumberLength = 15;

    private readonly List<Address> _addresses = new();

    public string Email { get; private set; } = null!;
    
    public string FirstName { get; private set; } = null!;

    public string LastName { get; private set; } = null!;

    public string FullName => $"{FirstName} {LastName}";    

    public string? AvatarUrl { get; private set; }

    public string? PhoneNumber { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    public DateTime? DeletedOnUtc { get; private set; }

    public bool Deleted { get; private set; }

    public IReadOnlyList<Address> Addresses => _addresses.AsReadOnly();

    private Customer()
    {
    }

    private Customer(
        CustomerId id,
        string email,
        string firstName,
        string lastName)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }

    public static ErrorOr<Customer> Create(
        string email,
        string firstName,
        string lastName)
    {
        List<Error> errors = ValidateNames(firstName, lastName);

        if (errors.Count > 0)
        {
            return errors;
        }

        var customer = new Customer(
            CustomerId.CreateUnique(),
            email,
            firstName,
            lastName);

        customer.RaiseDomainEvent(new CustomerCreatedDomainEvent(customer.Id, email));

        return customer;
    }

    public ErrorOr<Updated> UpdateDetails(
        string firstName,
        string lastName,
        string phoneNumber,
        string? avatarUrl)
    {
        List<Error> errors = ValidateNames(firstName, lastName);

        errors.AddRange(ValidatePhone(phoneNumber));

        if (errors.Count > 0)
        {
            return errors;
        }

        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        AvatarUrl = avatarUrl;

        return Result.Updated;
    }

    public ErrorOr<Address> AddAddress(
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
        if (isDefault)
        {
            foreach (var entity in _addresses)
            {
                entity.SetNormal();
            }
        }

        if (_addresses.Count is 0)
        {
            isDefault = true;
        }

        var createAddressResult = Address.Create(
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

        if (createAddressResult.IsError)
        {
            return createAddressResult.Errors;
        }

        var address = createAddressResult.Value;

        _addresses.Add(address);

        return address;
    }

    public ErrorOr<Address> UpdateAddress(
        AddressId addressId,
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
        var existingAddress = _addresses.FirstOrDefault(a => a.Id == addressId);

        if (existingAddress is null)
        {
            return Errors.Customer.AddressNotFound;
        }

        if (isDefault)
        {
            foreach (var address in _addresses)
            {
                address.SetNormal();
            }
        }

        var updateAddressResult = existingAddress.Update(
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

        if (updateAddressResult.IsError)
        {
            return updateAddressResult.Errors;
        }

        return existingAddress;
    }

    private static List<Error> ValidateNames(string firstName, string lastName)
    {
        List<Error> errors = new();

        if (firstName.Length is < MinFirstNameLength or > MaxFirstNameLength)
        {
            errors.Add(Errors.Customer.InvalidFirstNameLength);
        }

        if (lastName.Length is < MinLastNameLength or > MaxLastNameLength)
        {
            errors.Add(Errors.Customer.InvalidLastNameLength);
        }

        return errors;
    }

    private static List<Error> ValidatePhone(string phone)
    {
        List<Error> errors = new();

        if (phone.Length < MinPhoneNumberLength || phone.Length > MaxPhoneNumberLength)
        {
            errors.Add(Errors.Customer.InvalidPhoneNumberLength);
        }

        return errors;
    }
}

