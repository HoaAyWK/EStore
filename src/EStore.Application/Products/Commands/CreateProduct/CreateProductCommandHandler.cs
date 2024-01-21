using ErrorOr;
using EStore.Domain.BrandAggregate.Repositories;
using EStore.Domain.CategoryAggregate.Repositories;
using EStore.Domain.Common.Errors;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.Repositories;
using MediatR;

namespace EStore.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IBrandRepository brandRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _brandRepository = brandRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ErrorOr<Product>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        List<Error> errors = new();

        var brand = await _brandRepository.GetByIdAsync(request.BrandId);

        if (brand is null)
        {
            errors.Add(Errors.Product.BrandNotFound(request.BrandId));
        }

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

        if (category is null)
        {
            errors.Add(Errors.Product.CategoryNotFound(request.CategoryId));
        }

        var createProductResult = Product.Create(
            name: request.Name,
            description: request.Description,
            published: request.Published,
            price: request.Price,
            displayOrder: request.DisplayOrder,
            brandId: request.BrandId,
            categoryId: request.CategoryId,
            hasVariant: request.HasVariant);

        if (createProductResult.IsError)
        {
            errors.AddRange(createProductResult.Errors);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        var product = createProductResult.Value;

        await _productRepository.AddAsync(product);
        
        return product;
    }
}
