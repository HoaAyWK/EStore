using ErrorOr;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Commands.UpdateProductAttributeValue;

public record UpdateProductAttributeValueCommand(
    ProductAttributeValueId ProductAttributeValueId,
    ProductId ProductId,
    ProductAttributeId ProductAttributeId,
    string Name,
    string? Alias,
    decimal? PriceAdjustment)
    : IRequest<ErrorOr<Updated>>;
