using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Catalog.ProductAggregate;
using EStore.Domain.Catalog.ProductAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Products.Commands.UpdateProductAttribute;

public class UpdateProductAttributeCommandHandler
    : IRequestHandler<UpdateProductAttributeCommand, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductAttributeCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Product>> Handle(
        UpdateProductAttributeCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        var attribute = product.ProductAttributes.FirstOrDefault(a => a.Id == request.Id);

        if (attribute is null)
        {
            return Errors.Product.ProductAttributeNotFound;
        }

        attribute.Update(
            name: request.Name,
            alias: request.Alias,
            canCombine: request.CanCombine);
            
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product;
    }
}
