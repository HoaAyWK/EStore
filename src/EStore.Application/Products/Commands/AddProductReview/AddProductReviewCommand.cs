using ErrorOr;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.Entities;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Commands.AddProductReview;

public record AddProductReviewCommand(
    ProductId ProductId,
    string Title,
    string Content,
    int Rating,
    CustomerId OwnerId,
    ProductVariantId? ProductVariantId)
    : IRequest<ErrorOr<ProductReview>>;
