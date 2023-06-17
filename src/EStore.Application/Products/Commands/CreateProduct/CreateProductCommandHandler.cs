using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.Repositories;
using MediatR;

namespace EStore.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Product>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var createProductResult = Product.Create(
            name: request.Name,
            description: request.Description,
            published: request.Published,
            brandId: request.BrandId,
            categoryId: request.CategoryId);

        if (createProductResult.IsError)
        {
            return createProductResult.Errors;
        }

        var product = createProductResult.Value;

        await _productRepository.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return product;
    }
}
