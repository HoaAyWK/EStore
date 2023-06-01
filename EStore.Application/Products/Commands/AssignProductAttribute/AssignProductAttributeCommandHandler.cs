using ErrorOr;
using EStore.Domain.Catalog.ProductAggregate;
using EStore.Domain.Catalog.ProductAttributeAggregate;
using MediatR;

namespace EStore.Application.Products.Commands.AssignProductAttribute;

public class AssignProductAttributeCommandHandler
    : IRequestHandler<AssignProductAttributeCommand, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IProductAttributeRepository _productAttributeRepository;
    private readonly IProductAttributeReadRepository _productAttributeReadRepository;

    public AssignProductAttributeCommandHandler(
        IProductRepository productRepository,
        IProductReadRepository productReadRepository,
        IProductAttributeRepository productAttributeRepository,
        IProductAttributeReadRepository productAttributeReadRepository)
    {
        _productRepository = productRepository;
        _productReadRepository = productReadRepository;
        _productAttributeRepository = productAttributeRepository;
        _productAttributeReadRepository = productAttributeReadRepository;
    }

    public Task<ErrorOr<Product>> Handle(AssignProductAttributeCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
