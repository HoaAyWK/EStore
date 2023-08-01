using ErrorOr;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.Common.Models;
using EStore.Domain.Common.Errors;
using EStore.Domain.DiscountAggregate.ValueObjects;

namespace EStore.Domain.DiscountAggregate;

public sealed class Discount : AggregateRoot<DiscountId>, IAuditableEntity
{
    public const int MinNameLength = 2;
    public const int MaxNameLength = 100;
    public const decimal MinPercentage = 0;
    public const decimal MaxPercentage = 0.99m;
    public const decimal MinAmount = 0;

    public string Name { get; private set; } = null!;

    public bool UsePercentage { get; private set; }

    public decimal DiscountPercentage { get; private set; }

    public decimal DiscountAmount { get; private set; }

    public DateTime StartDateTime { get; private set; }

    public DateTime EndDateTime { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    private Discount()
    {
    }

    private Discount(
        DiscountId id,
        string name,
        bool usePercentage,
        decimal discountPercentage,
        decimal discountAmount,
        DateTime startDateTime,
        DateTime endDateTime) : base(id)
    {
        Name = name;
        UsePercentage = usePercentage;
        DiscountPercentage = discountPercentage;
        DiscountAmount = discountAmount;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
    }

    public static ErrorOr<Discount> Create(
        string name,
        bool usePercentage,
        decimal discountPercentage,
        decimal discountAmount,
        DateTime startDateTime,
        DateTime endDateTime)
    {
        var errors = new List<Error>();
        var validateNameResult = ValidateName(name);

        if (validateNameResult.IsError)
        {
            errors.Add(validateNameResult.FirstError);
        }

        var validateDiscountPercentageResult = ValidateDiscountPercentage(discountPercentage);

        if (validateDiscountPercentageResult.IsError)
        {
            errors.Add(validateDiscountPercentageResult.FirstError);
        }

        var validateDiscountAmount = ValidateDiscountAmount(discountAmount);

        if (validateDiscountAmount.IsError)
        {
            errors.Add(validateDiscountAmount.FirstError);
        }

        var validateDiscountDatesResult = ValidateDiscountDateTime(startDateTime, endDateTime);

        if (validateDiscountDatesResult.IsError)
        {
            errors.AddRange(validateDiscountDatesResult.Errors);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Discount(
            DiscountId.CreateUnique(),
            name,
            usePercentage,
            discountPercentage,
            discountAmount,
            startDateTime,
            endDateTime);
    }

    public ErrorOr<Updated> UpdateName(string name)
    {
        var validateNameResult = ValidateName(name);

        if (validateNameResult.IsError)
            return validateNameResult.FirstError;

        Name = name;

        return Result.Updated;
    }

    public void UpdateUsePercentage(bool usePercentage)
        => UsePercentage = usePercentage;

    public ErrorOr<Updated> UpdateDiscountPercentage(decimal discountPercentage)
    {
        var validateDiscountPercentageResult = ValidateDiscountPercentage(discountPercentage);

        if (validateDiscountPercentageResult.IsError)
            return validateDiscountPercentageResult.FirstError;

        DiscountPercentage = discountPercentage;

        return Result.Updated;
    }

    public ErrorOr<Updated> UpdateDiscountAmount(decimal discountAmount)
    {
        var validateDiscountAmountResult = ValidateDiscountAmount(discountAmount);

        if (validateDiscountAmountResult.IsError)
            return validateDiscountAmountResult.FirstError;

        DiscountAmount = discountAmount;

        return Result.Updated;
    }

    public ErrorOr<Updated> UpdateDates(DateTime startDate, DateTime endDate)
    {
        var validateDatesResult = ValidateDiscountDateTime(startDate, endDate);

        if (validateDatesResult.IsError)
            return validateDatesResult.Errors;

        StartDateTime = startDate;
        EndDateTime = endDate;

        return Result.Updated;
    }

    private static ErrorOr<Success> ValidateName(string name)
    {
        if (name.Length is < MinNameLength or > MaxNameLength)
        {
            return Errors.Discount.InvalidNameLength;
        }

        return Result.Success;
    }

    private static ErrorOr<Success> ValidateDiscountPercentage(decimal percentage)
    {
        if (percentage is < MinPercentage or > MaxPercentage)
        {
            return Errors.Discount.InvalidDiscountPercentage;
        }

        return Result.Success;
    }

    private static ErrorOr<Success> ValidateDiscountAmount(decimal amount)
    {
        if (amount < MinAmount)
        {
            return Errors.Discount.InvalidDiscountAmount;
        }

        return Result.Success;
    }

    private static ErrorOr<Success> ValidateDiscountDateTime(DateTime startDate, DateTime endDate)
    {
        var errors = new List<Error>();

        if (startDate < DateTime.UtcNow)
        {
            errors.Add(Errors.Discount.InvalidDiscountStartDate);
        }

        if (endDate < DateTime.UtcNow)
        {
            errors.Add(Errors.Discount.InvalidDiscountEndDate);
        }

        if (endDate <= startDate)
        {
            errors.Add(Errors.Discount.InvalidDiscountDates);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return Result.Success;
    }
}
