using ErrorOr;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Commands.UpdateVariant;

public record UpdateVariantCommand(
    ProductId ProductId,
    ProductVariantId ProductVariantId,
    int StockQuantity,
    decimal? Price,
    bool IsActive,
    List<Guid> AssignedImageIds)
    : IRequest<ErrorOr<Updated>>;
