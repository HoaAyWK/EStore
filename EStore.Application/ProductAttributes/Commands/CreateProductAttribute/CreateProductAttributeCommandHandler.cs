using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Catalog.ProductAttributeAggregate;
using MediatR;

namespace EStore.Application.ProductAttributes.Commands.CreateProductAttribute;

public class CreateProductAttributeCommandHandler
    : IRequestHandler<CreateProductAttributeCommand, ErrorOr<ProductAttribute>>
{
    private readonly IProductAttributeRepository _productAttributeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductAttributeCommandHandler(
        IProductAttributeRepository productAttributeRepository,
        IUnitOfWork unitOfWork)
    {
        _productAttributeRepository = productAttributeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<ProductAttribute>> Handle(
        CreateProductAttributeCommand request,
        CancellationToken cancellationToken)
    {
        var productAttribute = ProductAttribute.Create(
            request.Name,
            request.Description,
            request.Alias);
        
        await _productAttributeRepository.AddAsync(productAttribute);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return productAttribute;
    }
}
