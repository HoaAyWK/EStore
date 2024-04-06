using ErrorOr;
using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.Common.Errors;
using EStore.Domain.Common.Models;

namespace EStore.Domain.BrandAggregate;

public sealed class Brand : AggregateRoot<BrandId>, IAuditableEntity, ISoftDeletableEntity
{
    public const int MinNameLength = 2;

    public const int MaxNameLength = 100;

    public string Name { get; private set; } = null!;

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    public DateTime? DeletedOnUtc { get; private set; }

    public bool Deleted { get; private set; }

    private Brand()
    {
    }

    private Brand(
        BrandId brandId,
        string name)
        : base(brandId)
    {
        Name = name;
    }

    public static ErrorOr<Brand> Create(string name)
    {
        var errors = ValidateName(name);

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Brand(
            BrandId.CreateUnique(),
            name);
    }

    public ErrorOr<Updated> UpdateName(string name)
    {
        var errors = ValidateName(name);

        if (errors.Count > 0)
        {
            return errors;
        }

        Name = name;

        return Result.Updated;
    }


    private static List<Error> ValidateName(string name)
    {
        List<Error> errors = new();

        if (name.Length is < MinNameLength or > MaxNameLength)
        {
            errors.Add(Errors.Brand.InvalidNameLength);
        }

        return errors;
    }
}
