using ErrorOr;
using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.CategoryAggregate.ValueObjects;
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
    int DisplayOrder,
    BrandId BrandId,
    CategoryId CategoryId,
    int? StockQuantity,
    decimal? SpecialPrice,
    DateTime? SpecialPriceStartDate,
    DateTime? SpecialPriceEndDate) : IRequest<ErrorOr<Product>>;
