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

    public const int MinTitleLength = 1;

    public const int MaxTitleLength = 100;

    public const int MinContentLength = 1;

    public const int MaxContentLength = 2000; 

    private readonly List<ProductReviewComment> _reviewComments = new();

    public string Title { get; private set; } = null!;

    public string Content { get; private set; } = null!;

    public int Rating { get; private set; }

    public CustomerId OwnerId { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; private set; }

    public string RawAttributes { get; private set; } = default!;

    public IReadOnlyList<ProductReviewComment> ReviewComments => _reviewComments.AsReadOnly();

    private ProductReview(
        ProductReviewId id,
        string title,
        string content,
        int rating,
        string rawAttributes,
        CustomerId ownerId)
        : base(id)
    {
        Title = title;
        Content = content;
        Rating = rating;
        RawAttributes = rawAttributes;
        OwnerId = ownerId;
    }

    public static ErrorOr<ProductReview> Create(
        string title,
        string content,
        int rating,
        string rawAttributes,
        CustomerId customerId)
    {
        var errors = ValidateRatingValue(rating);

        errors.AddRange(ValidateTitle(title));
        errors.AddRange(ValidateContent(content));

        if (errors.Count > 0)
        {
            return errors;
        }

        return new ProductReview(
            ProductReviewId.CreateUnique,
            title,
            content,
            rating,
            rawAttributes,
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

    private static List<Error> ValidateTitle(string title)
    {
        var errors = new List<Error>();

        if (title.Length < MinTitleLength || title.Length > MaxTitleLength)
        {
            errors.Add(Errors.Product.InvalidReviewTitleLength);
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
