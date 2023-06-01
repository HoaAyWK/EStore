using ErrorOr;
using EStore.Domain.Catalog.ProductAttributeAggregate;
using MediatR;

namespace EStore.Application.ProductAttributes.Commands.CreateProductAttribute;

public record CreateProductAttributeCommand(
    string Name,
    string? Description,
    string? Alias) : IRequest<ErrorOr<ProductAttribute>>;
