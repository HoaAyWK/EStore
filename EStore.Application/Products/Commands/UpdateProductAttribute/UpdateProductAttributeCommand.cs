using ErrorOr;
using EStore.Domain.Catalog.ProductAggregate;
using EStore.Domain.Catalog.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Commands.UpdateProductAttribute;

public record UpdateProductAttributeCommand(
    ProductAttributeId Id,
    ProductId ProductId,
    string Name,
    string? Alias,
    bool CanCombine)
    : IRequest<ErrorOr<Product>>;
