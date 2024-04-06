using ErrorOr;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.Common.Errors;
using EStore.Domain.Common.Models;
using EStore.Domain.Common.ValueObjects;
using EStore.Domain.CustomerAggregate.Events;
using EStore.Domain.CustomerAggregate.ValueObjects;

namespace EStore.Domain.CustomerAggregate;

public sealed class Customer : AggregateRoot<CustomerId>, IAuditableEntity, ISoftDeletableEntity
{
    public const int MinFirstNameLength = 1;
    
    public const int MaxFirstNameLength = 100;

    public const int MinLastNameLength = 1;

    public const int MaxLastNameLength = 100;

    public const int PhoneNumberLength = 10;

    public string Email { get; private set; } = null!;
    
    public string FirstName { get; private set; } = null!;

    public string LastName { get; private set; } = null!;

    public string FullName => $"{FirstName} {LastName}";

    public Address? Address { get; private set; }

    public string? AvatarUrl { get; private set; }

    public string? PhoneNumber { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    public DateTime? DeletedOnUtc { get; }

    public bool Deleted { get; }

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
        string avatarUrl,
        string street,
        string city,
        string state,
        string country)
    {
        List<Error> errors = ValidateNames(firstName, lastName);
        var createAddressResult = Address.Create(street, city, state, country);

        errors.AddRange(ValidatePhone(phoneNumber));

        if (createAddressResult.IsError)
        {
            errors.AddRange(createAddressResult.Errors);
        }

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

        if (phone.Length != PhoneNumberLength)
        {
            errors.Add(Errors.Customer.InvalidPhoneNumberLength);
        }

        return errors;
    }
}

