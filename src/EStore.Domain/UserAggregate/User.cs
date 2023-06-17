using ErrorOr;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.Common.Errors;
using EStore.Domain.Common.Models;
using EStore.Domain.UserAggregate.ValueObjects;

namespace EStore.Domain.UserAggregate;

public sealed class User : AggregateRoot<UserId>, IAuditableEntity
{
    public const int MinFirstNameLength = 1;
    
    public const int MaxFirstNameLength = 100;

    public const int MinLastNameLength = 1;

    public const int MaxLastNameLength = 100;

    public const int MinEmailLength = 3;

    public string Email { get; private set; } = null!;
    
    public string FirstName { get; private set; } = null!;

    public string LastName { get; private set; } = null!;

    public string FullName => $"{FirstName} {LastName}";

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    private User()
    {
    }

    private User(
        UserId id,
        string email,
        string firstName,
        string lastName)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }

    public static ErrorOr<User> Create(
        string email,
        string firstName,
        string lastName)
    {
        List<Error> errors = ValidateNames(firstName, lastName);

        if (email.Length < MinEmailLength)
        {
            errors.Add(Errors.User.InvalidEmailLength);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new User(
            UserId.CreateUnique(),
            email,
            firstName,
            lastName);
    }


    public ErrorOr<Updated> UpdateDetails(string firstName, string lastName)
    {
        List<Error> errors = ValidateNames(firstName, lastName);

        if (errors.Count > 0)
        {
            return errors;
        }

        FirstName = firstName;
        LastName = lastName;

        return Result.Updated;
    }


    private static List<Error> ValidateNames(string firstName, string lastName)
    {
        List<Error> errors = new();

        if (firstName.Length is < MinFirstNameLength or > MaxFirstNameLength)
        {
            errors.Add(Errors.User.InvalidFirstNameLength);
        }

        if (lastName.Length is < MinLastNameLength or > MaxLastNameLength)
        {
            errors.Add(Errors.User.InvalidLastNameLength);
        }

        return errors;
    }
}
