using ErrorOr;
using EStore.Domain.Common.Errors;
using EStore.Domain.ProductAggregate.Repositories;
using MediatR;

namespace EStore.Application.Products.Commands.UpdateProductVariant;

public class UpdateProductVariantCommandHandler
    : IRequestHandler<UpdateProductVariantCommand, ErrorOr<Updated>>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductVariantCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdateProductVariantCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        var updateProductVariantResult = product.UpdateProductVariant(
            productVariantId: request.ProductVariantId,
            stockQuantity: request.StockQuantity,
            isActive: request.IsActive,
            imageIds: request.ImageIds);

        if (updateProductVariantResult.IsError)
        {
            return updateProductVariantResult.Errors;
        }

        return Result.Updated;
    }
}
