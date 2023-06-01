using ErrorOr;
using EStore.Domain.Catalog.ProductAggregate;
using MediatR;

namespace EStore.Application.Products.Commands.AssignProductAttribute;

public record AssignProductAttributeCommand(
    string ProductId,
    string ProductAttributeId)
    : IRequest<ErrorOr<Product>>;
