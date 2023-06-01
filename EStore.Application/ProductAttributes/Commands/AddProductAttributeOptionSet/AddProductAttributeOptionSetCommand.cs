using ErrorOr;
using EStore.Domain.Catalog.ProductAttributeAggregate;
using MediatR;

namespace EStore.Application.ProductAttributes.Commands.AddProductAttributeOptionSet;

public record AddProductAttributeOptionSetCommand(
    string ProductAttributeId,
    string Name)
    : IRequest<ErrorOr<ProductAttributeOptionSet>>;
