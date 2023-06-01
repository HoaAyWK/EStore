using ErrorOr;
using EStore.Domain.Catalog.ProductAggregate;
using MediatR;

namespace EStore.Application.Products.Commands.AddProductImage;

public record AddProductImageCommand(
    string ProductId,
    string ImageUrl,
    bool? IsMain,
    int DisplayOrder) : IRequest<ErrorOr<Product>>;
