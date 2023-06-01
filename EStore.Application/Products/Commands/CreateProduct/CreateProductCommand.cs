using ErrorOr;
using EStore.Domain.Catalog.ProductAggregate;
using MediatR;

namespace EStore.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    bool Published,
    string BrandId,
    string CategoryId)
    : IRequest<ErrorOr<Product>>;
