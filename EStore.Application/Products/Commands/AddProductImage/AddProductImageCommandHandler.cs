using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.ProductAggregate;
using MediatR;
using EStore.Domain.Common.Errors;
using EStore.Domain.ProductAggregate.Repositories;

namespace EStore.Application.Products.Commands.AddProductImage;

public class AddProductImageCommandHandler
    : IRequestHandler<AddProductImageCommand, ErrorOr<Updated>>
{
    private readonly IProductReadRepository _productReadRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddProductImageCommandHandler(
        IProductReadRepository productReadRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productReadRepository = productReadRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
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
        
        await _unitOfWork.SaveChangesAsync();

        return Result.Updated;
    }
}
