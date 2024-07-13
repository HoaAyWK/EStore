using ErrorOr;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.Entities;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Commands.EditProductReview;

public record EditProductReviewCommand(
    ProductId ProductId,
    ProductVariantId? ProductVariantId,
    ProductReviewId ProductReviewId,
    CustomerId OwnerId,
    string Content,
    int Rating)
    : IRequest<ErrorOr<ProductReview>>;
