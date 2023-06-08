using System.Text;
using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Catalog.ProductAggregate.Repositories;
using EStore.Domain.Catalog.ProductAggregate.ValueObjects;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Products.Commands.UpdateVariant;

public class UpdateVariantCommandHandler
    : IRequestHandler<UpdateVariantCommand, ErrorOr<Updated>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVariantCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdateVariantCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        var variant = product.ProductVariants.FirstOrDefault(
            v => v.Id == request.ProductVariantId);

        if (variant is null)
        {
            return Errors.Product.ProductVariantNotFound;
        }

        variant.UpdateDetails(
            request.StockQuantity,
            request.Price,
            request.IsActive);

        StringBuilder assignedImageIds = new StringBuilder("");

        if (request.AssignedImageIds is not null)
        {
            for (int i = 0; i < request.AssignedImageIds.Count; i++)
            {
                var assignedId = request.AssignedImageIds.ElementAt(i);
                var productImageId = product.Images.FirstOrDefault(
                    image => image.Id == ProductImageId.Create(assignedId));

                if (productImageId is null)
                {
                    return Errors.Product.ProductImageNotFound;
                }

                if (i is 0)
                {
                    assignedImageIds.Append(assignedId.ToString().ToLower());
                }
                else
                {
                    assignedImageIds.Append($" {assignedId.ToString().ToLower()}");
                }
            }

            variant.UpdateImages(assignedImageIds.ToString());
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}
