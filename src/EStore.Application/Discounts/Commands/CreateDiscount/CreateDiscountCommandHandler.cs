using ErrorOr;
using EStore.Domain.DiscountAggregate;
using EStore.Domain.DiscountAggregate.Repositories;
using MediatR;

namespace EStore.Application.Discounts.Commands.CreateDiscount;

public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, ErrorOr<Discount>>
{
    private readonly IDiscountRepository _discountRepository;

    public CreateDiscountCommandHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<ErrorOr<Discount>> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {
        var createDiscountResult = Discount.Create(
            request.Name,
            request.UsePercentage,
            request.DiscountPercentage,
            request.DiscountAmount,
            request.StartDate,
            request.EndDate);
        
        if (createDiscountResult.IsError)
        {
            return createDiscountResult.Errors;
        }

        var discount = createDiscountResult.Value;

        await _discountRepository.AddAsync(discount);

        return discount;
    }
}
