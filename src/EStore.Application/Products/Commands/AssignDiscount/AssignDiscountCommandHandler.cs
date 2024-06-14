using ErrorOr;
using EStore.Domain.Common.Errors;
using EStore.Domain.DiscountAggregate.Repositories;
using EStore.Domain.ProductAggregate.Repositories;
using MediatR;

namespace EStore.Application.Products.Commands.AssignDiscount;

public class AssignDiscountCommandHandler : IRequestHandler<AssignDiscountCommand, ErrorOr<Success>>
{
    private readonly IProductRepository _productRepository;
    private readonly IDiscountRepository _discountRepository;

    public AssignDiscountCommandHandler(
        IProductRepository productRepository,
        IDiscountRepository discountRepository)
    {
        _productRepository = productRepository;
        _discountRepository = discountRepository;
    }

    public async Task<ErrorOr<Success>> Handle(
        AssignDiscountCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        
        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        if (request.DiscountId is not null)
        {
            var discount = await _discountRepository.GetByIdAsync(request.DiscountId);

            if (discount is null)
            {
                return Errors.Discount.NotFound;
            }
        }

        product.AssignDiscount(request.DiscountId);

        return Result.Success;
    }
}