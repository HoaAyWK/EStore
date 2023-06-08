using ErrorOr;
using EStore.Domain.Catalog.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Commands.AddProductImage;

public record AddProductImageCommand(
    ProductId ProductId,
    string ImageUrl,
    bool? IsMain,
    int DisplayOrder) : IRequest<ErrorOr<Updated>>;
