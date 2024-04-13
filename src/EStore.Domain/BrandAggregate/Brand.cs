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

    public string? ImageUrl { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    public DateTime? DeletedOnUtc { get; private set; }

    public bool Deleted { get; private set; }

    private Brand()
    {
    }

    private Brand(
        BrandId brandId,
        string name,
        string? imageUrl)
        : base(brandId)
    {
        Name = name;
        ImageUrl = imageUrl;
    }

    public static ErrorOr<Brand> Create(string name, string? imageUrl)
    {
        var errors = ValidateName(name);

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Brand(
            BrandId.CreateUnique(),
            name,
            imageUrl);
    }

    public ErrorOr<Updated> UpdateDetails(string name, string? imageUrl)
    {
        var errors = ValidateName(name);

        if (errors.Count > 0)
        {
            return errors;
        }

        Name = name;
        ImageUrl = imageUrl;

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
