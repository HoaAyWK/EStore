using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Products.Commands.UpdateProductAttributeValue;

public class UpdateProductAttributeValueCommandHandler
    : IRequestHandler<UpdateProductAttributeValueCommand, ErrorOr<Updated>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductAttributeValueCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdateProductAttributeValueCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        var attribute = product.ProductAttributes.FirstOrDefault(
            a => a.Id == request.ProductAttributeId);

        if (attribute is null)
        {
            return Errors.Product.ProductAttributeNotFound;
        }

        var attributeValue = attribute.ProductAttributeValues.FirstOrDefault(
            v => v.Id == request.ProductAttributeValueId);

        if (attributeValue is null)
        {
            return Errors.Product.ProductAttributeValueNotFound;
        }

        attributeValue.UpdateDetails(
            request.Name,
            request.Alias,
            request.PriceAdjustment ?? 0);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}
