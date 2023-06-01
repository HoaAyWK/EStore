using ErrorOr;
using MediatR;

namespace EStore.Application.ProductAttributes.Commands.DeleteProductAttribute;

public record DeleteProductAttributeCommand(
    string ProductAttributeId)
    : IRequest<ErrorOr<DeleteProductAttributeResult>>;
