using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Catalog.ProductAggregate;
using MediatR;
using EStore.Domain.Common.Errors;
using EStore.Domain.Catalog.ProductAggregate.ValueObjects;

namespace EStore.Application.Products.Commands.AddProductImage;

public class AddProductImageCommandHandler
    : IRequestHandler<AddProductImageCommand, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddProductImageCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Product>> Handle(
        AddProductImageCommand request,
        CancellationToken cancellationToken)
    {
        ProductId productId = ProductId.Create(new Guid(request.ProductId));
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        bool isMainImage = request.IsMain ?? false;

        if (product.Images.Count is 0)
        {
            isMainImage = true;
        }
        else if (request.IsMain is true)
        {
            foreach (ProductImage image in product.Images)
            {
                if (image.IsMain is true)
                {
                    image.SetNormal();
                    break;
                }
            }
        }

        product.AddProductImage(
            ProductImage.Create(request.ImageUrl, isMainImage, request.DisplayOrder));
        
        await _unitOfWork.SaveChangesAsync();

        return product;
    }
}
