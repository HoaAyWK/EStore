using ErrorOr;
using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.ProductAggregate;
using MediatR;

namespace EStore.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    bool Published,
    int DisplayOrder,
    BrandId BrandId,
    CategoryId CategoryId)
    : IRequest<ErrorOr<Product>>;