using ErrorOr;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Commands.AddProductAttributeValue;

public record AddProductAttributeValueCommand(
    ProductId ProductId,
    ProductAttributeId ProductAttributeId,
    string Name,
    string? Color,
    decimal? PriceAdjustment,
    int DisplayOrder)
    : IRequest<ErrorOr<Product>>;
