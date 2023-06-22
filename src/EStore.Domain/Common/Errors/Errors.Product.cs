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
                $"{Domain.ProductAggregate.Product.MinNameLength} and " +
                $"{Domain.ProductAggregate.Product.MaxNameLength} characters.");

        public static Error InvalidPrice = Error.Validation(
            code: "Product.InvalidPrice",
            description: "Product price must be larger than " +
                $"{Domain.ProductAggregate.Product.MinPrice}.");

        public static Error InvalidSpecialPriceStartDate = Error.Validation(
            code: "Product.InvalidSpecialPriceStartDate",
            description: "Product Special Price Start Date can not be less than the current date time.");

        public static Error InvalidSpecialPriceEndDate = Error.Validation(
            code: "Product.InvalidSpecialPriceEndDate",
            description: "Product Special Price End Date must be larger than Product Special Price Start Date.");
    }
}
