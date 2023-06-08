using ErrorOr;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    ProductId Id,
    string Name,
    string Description,
    decimal Price,
    bool Published,
    string BrandId,
    string CategoryId,
    decimal? SpecialPrice,
    DateTime? SpecialPriceStartDate,
    DateTime? SpecialPriceEndDate) : IRequest<ErrorOr<Product>>;
