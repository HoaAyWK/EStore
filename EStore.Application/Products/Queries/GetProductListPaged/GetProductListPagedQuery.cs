using ErrorOr;
using EStore.Domain.Catalog.ProductAggregate;
using MediatR;

namespace EStore.Application.Products.Queries.GetProductListPaged;

public record GetProductListPagedQuery()
    : IRequest<ErrorOr<List<Product>>>;
