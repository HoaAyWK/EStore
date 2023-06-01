using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Common.Errors;
using EStore.Domain.Catalog.ProductAttributeAggregate;
using EStore.Domain.Catalog.ProductAttributeAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.ProductAttributes.Commands.UpdateProductAttribute;

public class UpdateProductAttributeCommandHandler
    : IRequestHandler<UpdateProductAttributeCommand, ErrorOr<ProductAttribute>>
{
    private readonly IProductAttributeReadRepository _productAttributeReadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductAttributeCommandHandler(
        IProductAttributeReadRepository productAttributeReadRepository,
        IUnitOfWork unitOfWork)
    {
        _productAttributeReadRepository = productAttributeReadRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<ProductAttribute>> Handle(
        UpdateProductAttributeCommand request,
        CancellationToken cancellationToken)
    {
        ProductAttributeId productAttributeId = ProductAttributeId.Create(
            new Guid(request.ProductAttributeId));

        var productAttribute = await _productAttributeReadRepository.GetByIdAsync(productAttributeId);

        if (productAttribute is null)
        {
            return Errors.ProductAttribute.NotFound;
        }

        productAttribute.UpdateDetails(
            name: request.Name,
            description: request.Description,
            alias: request.Alias);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return productAttribute;
    }
}
