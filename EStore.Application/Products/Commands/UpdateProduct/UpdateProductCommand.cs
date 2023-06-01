using ErrorOr;
using EStore.Domain.Catalog.ProductAggregate;
using MediatR;

namespace EStore.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    string ProductId,
    string Name,
    string Description,
    decimal Price,
    bool Published,
    string BrandId,
    string CategoryId,
    decimal? SpecialPrice,
    DateTime? SpecialPriceStartDate,
    DateTime? SpecialPriceEndDate) : IRequest<ErrorOr<Product>>;
