using ErrorOr;
using EStore.Domain.ProductAggregate.Entities;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Commands.AddProductVariant;

public record AddProductVariantCommand(
    ProductId ProductId,
    int StockQuantity,
    List<SelectedAttribute> SelectedAttributes,
    bool IsActive,
    string? AssignedProductImageIds)
    : IRequest<ErrorOr<ProductVariant>>;

public record SelectedAttribute(
    ProductAttributeId ProductAttributeId,
    ProductAttributeValueId ProductAttributeValueId);

