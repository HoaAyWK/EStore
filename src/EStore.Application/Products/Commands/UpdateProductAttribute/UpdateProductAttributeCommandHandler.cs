using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.Repositories;
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

        var updateProductAttributeResult = product.UpdateProductAttribute(
            request.Id,
            request.Name,
            request.Alias,
            request.CanCombine);

        if (updateProductAttributeResult.IsError)
        {
            return updateProductAttributeResult.Errors;
        }
            
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product;
    }
}
