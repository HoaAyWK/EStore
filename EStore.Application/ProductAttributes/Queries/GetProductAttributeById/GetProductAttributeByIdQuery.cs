using ErrorOr;
using EStore.Domain.Catalog.ProductAttributeAggregate;
using MediatR;

namespace EStore.Application.ProductAttributes.Queries.GetProductAttributeById;

public record GetProductAttributeByIdQuery(string ProductAttributeId)
    : IRequest<ErrorOr<ProductAttribute>>;
