using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Common.Errors;
using EStore.Domain.Catalog.ProductAttributeAggregate;
using EStore.Domain.Catalog.ProductAttributeAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.ProductAttributes.Commands.AddProductAttributeOptionSet;

public class AddProductAttributeOptionSetCommandHandler
    : IRequestHandler<AddProductAttributeOptionSetCommand, ErrorOr<ProductAttributeOptionSet>>
{
    private readonly IProductAttributeReadRepository _productAttributeReadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddProductAttributeOptionSetCommandHandler(
        IProductAttributeReadRepository productAttributeReadRepository,
        IUnitOfWork unitOfWork)
    {
        _productAttributeReadRepository = productAttributeReadRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<ProductAttributeOptionSet>> Handle(
        AddProductAttributeOptionSetCommand request,
        CancellationToken cancellationToken)
    {
        var productAttribute = await _productAttributeReadRepository.GetByIdAsync(
            ProductAttributeId.Create(new Guid(request.ProductAttributeId)));
        
        if (productAttribute is null)
        {
            return Errors.ProductAttribute.NotFound;
        }

        var productAttributeOptionSet = ProductAttributeOptionSet.Create(request.Name);

        productAttribute.AddOptionSet(productAttributeOptionSet);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return productAttributeOptionSet;
    }
}
