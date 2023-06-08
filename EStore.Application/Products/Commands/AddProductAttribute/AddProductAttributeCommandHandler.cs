using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Products.Commands.AddProductAttribute;

public class AddProductAttributeCommandHandler
    : IRequestHandler<AddProductAttributeCommand, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddProductAttributeCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Product>> Handle(
        AddProductAttributeCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        product.AddProductAttribute(ProductAttribute.Create(
            request.Name,
            request.Alias,
            request.CanCombine));
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product;
    }
}
