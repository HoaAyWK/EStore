using ErrorOr;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Commands.UpdateProductAttribute;

public record UpdateProductAttributeCommand(
    ProductAttributeId Id,
    ProductId ProductId,
    string Name,
    bool CanCombine)
    : IRequest<ErrorOr<Product>>;
