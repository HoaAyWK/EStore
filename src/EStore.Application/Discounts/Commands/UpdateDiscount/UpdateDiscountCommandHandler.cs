using ErrorOr;
using EStore.Domain.DiscountAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Discounts.Commands.UpdateDiscount;

public class UpdateDiscountCommandHandler
    : IRequestHandler<UpdateDiscountCommand, ErrorOr<Updated>>
{
    private readonly IDiscountRepository _discountRepository;

    public UpdateDiscountCommandHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = await _discountRepository.GetByIdAsync(request.Id);

        if (discount is null)
        {
            return Errors.Discount.NotFound;
        }

        var errors = new List<Error>();
        var updateNameResult = discount.UpdateName(request.Name);
        
        if (updateNameResult.IsError)
        {
            errors.Add(updateNameResult.FirstError);
        }

        discount.UpdateUsePercentage(request.UsePercentage);

        var updateDiscountPercentageResult = discount.UpdateDiscountPercentage(request.DiscountPercentage);

        if (updateDiscountPercentageResult.IsError)
        {
            errors.Add(updateDiscountPercentageResult.FirstError);
        }

        var updateDiscountAmountResult = discount.UpdateDiscountAmount(request.DiscountAmount);

        if (updateDiscountAmountResult.IsError)
        {
            errors.Add(updateDiscountAmountResult.FirstError);
        }

        var updateDatesResult = discount.UpdateDates(request.StartDate, request.EndDate);

        if (updateDatesResult.IsError)
        {
            errors.AddRange(updateDatesResult.Errors);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return Result.Updated;
    }
}
