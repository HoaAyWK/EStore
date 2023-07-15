using ErrorOr;
using MediatR;
using EStore.Domain.Common.Errors;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductAggregate.Entities;

namespace EStore.Application.Products.Commands.AddProductImage;

public class AddProductImageCommandHandler
    : IRequestHandler<AddProductImageCommand, ErrorOr<Updated>>
{
    private readonly IProductRepository _productRepository;

    public AddProductImageCommandHandler(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(
        AddProductImageCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

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
            ProductImage.Create(
                request.ImageUrl,
                isMainImage,
                request.DisplayOrder));
                
        return Result.Updated;
    }
}
