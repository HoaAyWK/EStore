using ErrorOr;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.Common.Errors;
using EStore.Domain.Common.Models;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.ProductAggregate.Entities;

public class ProductReviewComment : Entity<ProductReviewCommentId>, IAuditableEntity
{
    public const int MinContentLength = 1;

    public string Content { get; private set; } = default!;

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    public CustomerId OwnerId { get; private set; }

    public ProductReviewCommentId? ParentId { get; private set; }

    private ProductReviewComment(
        ProductReviewCommentId id,
        string content,
        CustomerId ownerId)
        : base(id)
    {
        Content = content;
        OwnerId = ownerId;
    }

    public static ErrorOr<ProductReviewComment> Create(
        string content,
        CustomerId customerId)
    {
        var validateResult = ValidateContent(content);

        if (validateResult.IsError)
        {
            return validateResult.Errors;
        }

        return new ProductReviewComment(
            ProductReviewCommentId.CreateUnique,
            content,
            customerId);
    }

    public void UpdateParent(ProductReviewCommentId parentId)
    {
        ParentId = parentId;
    }

    private static ErrorOr<Success> ValidateContent(string content)
    {
        if (content.Length < MinContentLength)
        {
            return Errors.Product.InvalidProductReviewCommentContentLength;
        }

        return Result.Success;
    }
}
