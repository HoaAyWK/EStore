using ErrorOr;
using EStore.Domain.Catalog.ProductAggregate;
using EStore.Domain.Catalog.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Commands.DeleteAttributeValue;

public record DeleteAttributeValueCommand(
    ProductId ProductId,
    ProductAttributeId ProductAttributeId,
    ProductAttributeValueId ProductAttributeValueId)
    : IRequest<ErrorOr<Product>>;
