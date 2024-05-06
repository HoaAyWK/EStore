using ErrorOr;
using EStore.Domain.Common.Abstractions;
using EStore.Domain.Common.Errors;
using EStore.Domain.Common.Models;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;

namespace EStore.Domain.ProductAggregate.Entities;

public class ProductReview : Entity<ProductReviewId>, IAuditableEntity
{
    public const int MinRatingValue = 1;

    public const int MaxRatingValue = 5;

    public const int MinContentLength = 1;

    public const int MaxContentLength = 2000; 

    private readonly List<ProductReviewComment> _reviewComments = new();

    public string Content { get; private set; } = null!;

    public int Rating { get; private set; }

    public CustomerId OwnerId { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    public string RawAttributes { get; private set; } = default!;

    public string? RawAttributeSelection { get; private set; }

    public IReadOnlyList<ProductReviewComment> ReviewComments => _reviewComments.AsReadOnly();

    private ProductReview(
        ProductReviewId id,
        string content,
        int rating,
        string rawAttributes,
        string? rawAttributeSelection,
        CustomerId ownerId)
        : base(id)
    {
        Content = content;
        Rating = rating;
        RawAttributes = rawAttributes;
        RawAttributeSelection = rawAttributeSelection;
        OwnerId = ownerId;
    }

    public static ErrorOr<ProductReview> Create(
        string content,
        int rating,
        string rawAttributes,
        string? rawAttributeSelection,
        CustomerId customerId)
    {
        var errors = ValidateRatingValue(rating);

        errors.AddRange(ValidateContent(content));

        if (errors.Count > 0)
        {
            return errors;
        }

        return new ProductReview(
            ProductReviewId.CreateUnique,
            content,
            rating,
            rawAttributes,
            rawAttributeSelection,
            customerId);
    }

    public void AddReviewComment(ProductReviewComment reviewComment)
    {
        _reviewComments.Add(reviewComment);
    }

    public void RemoveReviewComment(ProductReviewComment reviewComment)
    {
        _reviewComments.Remove(reviewComment);
    }

    private static List<Error> ValidateRatingValue(int rating)
    {
        var errors = new List<Error>();

        if (rating < MinRatingValue || rating > MaxRatingValue)
        {
            errors.Add(Errors.Product.InvalidRatingValue);
        }

        return errors;
    }

    private static List<Error> ValidateContent(string content)
    {
        var errors = new List<Error>();

        if (content.Length < MinContentLength || content.Length > MaxContentLength)
        {
            errors.Add(Errors.Product.InvalidReviewContentLength);
        }

        return errors;
    }
}
