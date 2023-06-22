using ErrorOr;
using EStore.Application.Products.Dtos;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(ProductId ProductId) : IRequest<ErrorOr<ProductDto>>;
