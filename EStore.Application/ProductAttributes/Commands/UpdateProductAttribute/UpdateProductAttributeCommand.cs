using ErrorOr;
using EStore.Domain.Catalog.ProductAttributeAggregate;
using MediatR;

namespace EStore.Application.ProductAttributes.Commands.UpdateProductAttribute;

public record UpdateProductAttributeCommand(
    string ProductAttributeId,
    string Name,
    string? Description,
    string? Alias)
    : IRequest<ErrorOr<ProductAttribute>>;
