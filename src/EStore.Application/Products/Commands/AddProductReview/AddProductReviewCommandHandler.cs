using ErrorOr;
using EStore.Domain.CustomerAggregate.Repositories;
using EStore.Domain.ProductAggregate.Entities;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Products.Commands.AddProductReview;

public class AddProductReviewCommandHandler
    : IRequestHandler<AddProductReviewCommand, ErrorOr<ProductReview>>
{
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;

    public AddProductReviewCommandHandler(
        IProductRepository productRepository,
        ICustomerRepository customerRepository)
    {
        _productRepository = productRepository;
        _customerRepository = customerRepository;
    }

    public async Task<ErrorOr<ProductReview>> Handle(
        AddProductReviewCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        var customer = await _customerRepository.GetByIdAsync(request.OwnerId);

        if (customer is null)
        {
            return Errors.Customer.NotFound;
        }

        // TODO: need to check if the customer has purchased the product

        return product.AddReview(
            request.ProductVariantId,
            request.Content,
            request.Rating,
            request.OwnerId);
    }
}
