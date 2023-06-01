using ErrorOr;
using EStore.Domain.Catalog.ProductAttributeAggregate;
using MediatR;

namespace EStore.Application.ProductAttributes.Queries.GetProductAttributeListPaged;

public record GetProductAttributeListPagedQuery()
    : IRequest<ErrorOr<List<ProductAttribute>>>;
