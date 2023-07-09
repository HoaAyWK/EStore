using ErrorOr;
using EStore.Domain.ProductVariantAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.ProductVariants.Commands.UpdateProductVariant;

public record UpdateProductVariantCommand(
    ProductVariantId Id,
    int StockQuantity,
    bool IsActive,
    List<Guid>? AssignedImageIds = null)
    : IRequest<ErrorOr<Updated>>;