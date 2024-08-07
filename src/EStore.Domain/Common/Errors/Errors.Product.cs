using ErrorOr;
using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.CategoryAggregate.ValueObjects;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class Product
    {
        public static Error NotFound = Error.NotFound(
            code: "Product.NotFound",
            description: "The product with specified identifier was not found.");

        public static Error NotFoundValidation(Guid id) => Error.Validation(
            code: "Product.NotFoundValidation",
            description: $"The product with identifier = {id} was not found.");

        public static Error BrandNotFound(BrandId brandId) =>
            Error.Validation(
                code: "Product.BrandNotFound",
                description: $"The brand with id = {brandId.Value} was not found.");

        public static Error CategoryNotFound(CategoryId categoryId) =>
            Error.Validation(
                code: "Product.CategoryNotFound",
                description: $"The category with id = {categoryId.Value} was not found.");

        public static Error InvalidStockQuantity => Error.Validation(
            code: "Product.InvalidStockQuantity",
            description: $"Stock quantity must be greater than or equal " +
                $"{Domain.ProductAggregate.Product.MinStockQuantity}.");

        public static Error UnprovidedSpecialPriceStartDate = Error.Validation(
            code: "Product.UnprovidedSpecialPriceStartDate",
            description: "Start date is required when Special price is provided.");

        public static Error UnprovidedSpecialPriceEndDate = Error.Validation(
            code: "Product.UnprovidedSpecialPriceEndDate",
            description: "End date is required when Special price is provided.");

        public static Error SpecialPriceStartDateLessThanEndDate = Error.Validation(
            code: "Product.SpecialPriceStartDateLessThanEndDate",
            description: "End date must be lagger than Start date.");

        public static Error DuplicateProductAttribute = Error.Validation(
            code: "Product.DuplicateProductAttribute",
            description: "The product attribute already assigned.");

        public static Error ProductAttributeNotFound = Error.Validation(
            "Product.ProductAttributeNotFound",
            "The product's attribute with specified identifier was not found.");

        public static Error ProductAttributeValueNotFound = Error.Validation(
            code: "Product.ProductAttributeValueNotFound",
            description: "The product's attribute value with specified identifier was not found.");

        public static Error ProductVariantNotFound = Error.Validation(
            code: "Product.ProductVariantNotFound",
            description: "The product's variant with specified identifier was not found.");

        public static Error ProductVariantNotFoundValidation(Guid id) => Error.Validation(
            code: "Product.ProductVariantNotFoundValidation",
            description: $"The product's variant with identifier = ${id} was not found.");

        public static Error ProductImageNotFound = Error.Validation(
            code: "Product.ProductImageNotFound",
            description: "The product's image with specified identifier was not found.");

        public static Error InvalidAttributeSelectionNumbers = Error.Validation(
            code: "Product.InvalidAttributeSelectionNumbers",
            description: "The number of selected attributes is not equal the number of product's attributes.");

        public static Error ProductVariantExisted = Error.Validation(
            code: "Product.ProductVariantExisted",
            description: "Product  attribute combination already existed.");

        public static Error ProductAttributeCannotCombine = Error.Validation(
            code: "Product.ProductAttributeCannotCombine",
            description: "Product attribute with specified identifier can not combine.");

        public static Error CannotCreateVariantWithTheSameAttribute = Error.Validation(
            code: "Product.CannotCreateVariantWithTheSameAttribute",
            description: "Can not create product variant with the same attribute.");


        public static Error InvalidNameLength = Error.Validation(
            code: "Product.InvalidNameLength",
            description: "Product name must be between " +
                $"{ProductAggregate.Product.MinNameLength} and " +
                $"{ProductAggregate.Product.MaxNameLength} characters.");

        public static Error InvalidPrice = Error.Validation(
            code: "Product.InvalidPrice",
            description: "Product price must be larger than " +
                $"{ProductAggregate.Product.MinPrice}.");

        public static Error InvalidSpecialPriceStartDate = Error.Validation(
            code: "Product.InvalidSpecialPriceStartDate",
            description: "Product Special Price Start Date can not be less than the current date time.");

        public static Error InvalidSpecialPriceEndDate = Error.Validation(
            code: "Product.InvalidSpecialPriceEndDate",
            description: "Product Special Price End Date must be larger than Product Special Price Start Date.");

        public static Error InvalidProductVariantPrice = Error.Validation(
            code: "Product.InvalidProductVariantPrice",
            description: "Price must be greater than or equal " +
                $"{ProductAggregate.Entities.ProductVariant.MinPrice}");

        public static Error InvalidProductVariantStockQuantity = Error.Validation(
            code: "Product.InvalidProductVariantStockQuantity",
            description: "Stock quantity must be greater than or equal " +
                $"{ProductAggregate.Entities.ProductVariant.MinStockQuantity}");

        public static Error DuplicateVariant = Error.Validation(
            code: "Product.DuplicateVariant",
            description: "Product variant already exists.");

        public static Error ProductCanNotHaveVariant = Error.Validation(
            code: "Product.ProductCanNotHaveVariant",
            description: "Product can not have variant.");

        public static Error NonVariantProductCannotHaveCombineAttributes = Error.Validation(
            code: "Product.NonVariantProduct",
            description: "Non variant product can not have combine attributes.");

        public static Error NonCombineProductAttributeCannotHaveMoreThanTwoValues = Error.Validation(
            code: "Product.NonCombineProductAttributeCannotHaveMoreThanTwoValues",
            description: "Non combine product attribute can not have more than two values.");

        public static Error ProductAttributeAlreadyHadValues = Error.Validation(
            code: "Product.ProductAttributeAlreadyHasValues",
            description: "Product attribute already had values.");

        public static Error ProductAlreadyHadVariants = Error.Validation(
            code: "Product.ProductAlreadyHadVariants",
            description: "Product already had variants.");

        public static Error InvalidProductReviewCommentContentLength = Error.Validation(
            code: "Product.InvalidProductReviewCommentContentLength",
            description: "Product review comment content must be at least " +
                $"{ProductAggregate.Entities.ProductReviewComment.MinContentLength} characters.");

        public static Error InvalidRatingValue = Error.Validation(
            code: "Product.InvalidRatingValue",
            description: "Rating value must be between " +
                $"{ProductAggregate.Entities.ProductReview.MinRatingValue} and " +
                $"{ProductAggregate.Entities.ProductReview.MaxRatingValue}.");

        public static Error InvalidReviewContentLength = Error.Validation(
            code: "Product.InvalidReviewContentLength",
            description: "Review content must be between " +
                $"{ProductAggregate.Entities.ProductReview.MinContentLength} and " +
                $"{ProductAggregate.Entities.ProductReview.MaxContentLength} characters.");

        public static Error ProductHadVariants = Error.Validation(
            code: "Product.ProductHadVariants",
            description: "Product already had variants, cannot turn to non-variants product.");

        public static Error ProductHadCombinableAttributes = Error.Validation(
            code: "Product.ProductHadCombinableAttributes",
            description: "Product already had combinable attributes, "
                + "cannot turn to non-combinable attributes product.");

        public static Error CustomerAlreadyReviewed = Error.Validation(
            code: "Product.CustomerAlreadyReviewed",
            description: "Customer already reviewed the product.");


        public static Error InvalidProductAttributeNameLength = Error.Validation(
            code: "Product.InvalidProductAttributeNameLength",
            description: "Product attribute name must be between " +
                $"{ProductAggregate.Entities.ProductAttribute.MinNameLength} and " +
                $"{ProductAggregate.Entities.ProductAttribute.MaxNameLength} characters.");

        public static Error NotEnoughStock(Guid id) => Error.Validation(
            code: "Product.NotEnoughStock",
            description: $"The product with id = {id} does not have enough stock.");

        public static Error ProductVariantNotEnoughStock(Guid id) => Error.Validation(
            code: "Product.ProductVariantNotEnoughStock",
            description: $"The product's variant with id = {id} does not have enough stock.");

        public static Error InvalidShortDescriptionLength = Error.Validation(
            code: "Product.InvalidShortDescriptionLength",
            description: "Product short description must be between " +
                $"{ProductAggregate.Product.MinShortDescriptionLength} and " +
                $"{ProductAggregate.Product.MaxShortDescriptionLength} characters.");

        public static Error NotPublishedYet = Error.Validation(
            code: "Product.NotPublishedYet",
            description: "The product is not published yet.");

        public static Error ProductHasNotHadImageYet = Error.Validation(
            code: "Product.ProductHasNotHadImageYet",
            description: "Product has not had any image yet.");

        public static Error ProductReviewNotFound = Error.Validation(
            code: "Product.ProductReviewNotFound",
            description: "Product review with specified identifier was not found.");

        public static Error CustomerCannotUpdateOthersReview = Error.Validation(
            code: "Product.CustomerCannotUpdateOthersReview",
            description: "Customer cannot update others' review.");
    }
}
