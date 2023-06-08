using ErrorOr;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Commands.DeleteAttributeValue;

public record DeleteAttributeValueCommand(
    ProductId ProductId,
    ProductAttributeId ProductAttributeId,
    ProductAttributeValueId ProductAttributeValueId)
    : IRequest<ErrorOr<Product>>;
