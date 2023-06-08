using ErrorOr;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Commands.AddProductAttribute;

public record AddProductAttributeCommand(
    ProductId ProductId,
    string Name,
    string? Alias,
    bool CanCombine)
    : IRequest<ErrorOr<Product>>;
