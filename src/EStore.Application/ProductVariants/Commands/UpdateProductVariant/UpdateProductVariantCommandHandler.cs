using System.Text;
using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Common.Errors;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.ProductVariantAggregate.Repositories;
using MediatR;

namespace EStore.Application.ProductVariants.Commands.UpdateProductVariant;

public class UpdateProductVariantCommandHandler
    : IRequestHandler<UpdateProductVariantCommand, ErrorOr<Updated>>
{
    private readonly IProductVariantRepository _productVariantRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductVariantCommandHandler(
        IProductVariantRepository productVariantRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productVariantRepository = productVariantRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdateProductVariantCommand request,
        CancellationToken cancellationToken)
    {
        var errors = new List<Error>();
        var productVariant = await _productVariantRepository.GetByIdAsync(request.Id);

        if (productVariant is null)
        {
            return Errors.ProductVariant.NotFound;
        }

        var updateDetailsResult = productVariant.UpdateDetails(
            request.StockQuantity,
            request.Price,
            request.IsActive);

        if (updateDetailsResult.IsError)
        {
            errors.AddRange(updateDetailsResult.Errors);
        }

        var product = await _productRepository.GetByIdAsync(productVariant.ProductId);

        if (product is null)
        {
            errors.Add(Errors.ProductVariant.ProductNotFound(productVariant.ProductId));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        StringBuilder assignedImageIds = new StringBuilder();

        if (request.AssignedImageIds is not null)
        {
            for (int i = 0; i < request.AssignedImageIds.Count; i++)
            {
                var assignedId = request.AssignedImageIds.ElementAt(i);
                var productImageId = product!.Images.FirstOrDefault(
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

            productVariant.UpdateAssignedImageIds(assignedImageIds.ToString());
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}
