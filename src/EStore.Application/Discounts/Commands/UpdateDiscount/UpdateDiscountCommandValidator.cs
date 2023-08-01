using EStore.Domain.DiscountAggregate;
using FluentValidation;

namespace EStore.Application.Discounts.Commands.UpdateDiscount;

public class UpdateDiscountCommandValidator : AbstractValidator<UpdateDiscountCommand>
{
    public UpdateDiscountCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Must(name => name.Length >= Discount.MinNameLength && name.Length <= Discount.MaxNameLength)
            .WithMessage("Discount Name must be between " +
                $"{Discount.MinNameLength} and " +
                $"{Discount.MaxNameLength} characters.");

        RuleFor(x => x.DiscountPercentage)
            .Must(percentage => percentage > Discount.MinPercentage && percentage < Discount.MaxPercentage)
            .WithMessage("Discount Percentage must be between " +
                $"{Discount.MinPercentage} and {Discount.MaxPercentage}.");

        RuleFor(x => x.DiscountAmount)
            .Must(amount => amount >= Discount.MinAmount)
            .WithMessage("Discount Amount must be larger than or equal " +
                $"{Discount.MinAmount}.");

        RuleFor(x => x.StartDate)
            .Must(startDate => startDate >= DateTime.UtcNow)
            .WithMessage("Discount Start Date must be larger than current date.");
        
        RuleFor(x => x.EndDate)
            .Must(endDate => endDate >= DateTime.UtcNow)
            .WithMessage("Discount End Date must be larger than current date.");
    }
}
