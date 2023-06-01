using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Common.Errors;
using EStore.Domain.Catalog.ProductAttributeAggregate;
using EStore.Domain.Catalog.ProductAttributeAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.ProductAttributes.Commands.DeleteProductAttribute;

public class DeleteProductAttributeCommandHandler
    : IRequestHandler<DeleteProductAttributeCommand, ErrorOr<DeleteProductAttributeResult>>
{
    private readonly IProductAttributeReadRepository _productAttributeReadRepository;
    private readonly IProductAttributeRepository _productAttributeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductAttributeCommandHandler(
        IProductAttributeReadRepository productAttributeReadRepository,
        IProductAttributeRepository productAttributeRepository,
        IUnitOfWork unitOfWork)
    {
        _productAttributeReadRepository = productAttributeReadRepository;
        _productAttributeRepository = productAttributeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<DeleteProductAttributeResult>> Handle(
        DeleteProductAttributeCommand request,
        CancellationToken cancellationToken)
    {
        var productAttribute = await _productAttributeReadRepository.GetByIdAsync(
            ProductAttributeId.Create(new Guid(request.ProductAttributeId)));

        if (productAttribute is null)
        {
            return Errors.ProductAttribute.NotFound;
        }

        _productAttributeRepository.Delete(productAttribute);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DeleteProductAttributeResult(productAttribute);
    }
}
