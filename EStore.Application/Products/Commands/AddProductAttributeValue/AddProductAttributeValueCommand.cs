using ErrorOr;
using EStore.Domain.Catalog.ProductAggregate;
using EStore.Domain.Catalog.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Commands.AddProductAttributeValue;

public record AddProductAttributeValueCommand(
    ProductId ProductId,
    ProductAttributeId ProductAttributeId,
    string Name,
    string? Alias,
    decimal? PriceAdjustment)
    : IRequest<ErrorOr<Product>>;
