using ErrorOr;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(ProductId ProductId) : IRequest<ErrorOr<Product>>;
