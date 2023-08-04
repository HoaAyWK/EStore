using ErrorOr;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Commands.UpdateProductVariant;

public record UpdateProductVariantCommand(
    ProductId ProductId,
    ProductVariantId ProductVariantId,
    int StockQuantity,
    bool IsActive,
    List<Guid>? ImageIds = null)
    : IRequest<ErrorOr<Updated>>;
