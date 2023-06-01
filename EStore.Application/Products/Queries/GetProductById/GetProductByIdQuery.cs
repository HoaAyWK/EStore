using ErrorOr;
using EStore.Domain.Catalog.ProductAggregate;
using EStore.Domain.Catalog.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(ProductId ProductId) : IRequest<ErrorOr<Product>>;
