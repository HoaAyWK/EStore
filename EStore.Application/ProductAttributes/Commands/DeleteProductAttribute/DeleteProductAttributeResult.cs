using EStore.Domain.Catalog.ProductAttributeAggregate;

namespace EStore.Application.ProductAttributes.Commands.DeleteProductAttribute;

public record DeleteProductAttributeResult(
    ProductAttribute ProductAttribute,
    bool Status = true,
    string Message = "Delete successfully.");