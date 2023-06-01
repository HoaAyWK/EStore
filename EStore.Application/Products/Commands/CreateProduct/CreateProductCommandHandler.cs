using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Catalog.BrandAggregate;
using EStore.Domain.Catalog.BrandAggregate.ValueObjects;
using EStore.Domain.Catalog.CategoryAggregate;
using EStore.Domain.Catalog.CategoryAggregate.ValueObjects;
using EStore.Domain.Catalog.ProductAggregate;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IBrandRepository brandRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _brandRepository = brandRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ErrorOr<Product>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        BrandId brandId = BrandId.Create(new Guid(request.BrandId));
        var brand = await _brandRepository.GetByIdAsync(brandId);

        if (brand is null)
        {
            return Errors.Brand.NotFound;
        }

        CategoryId categoryId = CategoryId.Create(new Guid(request.CategoryId));
        var category = await _categoryRepository.GetByIdAsync(categoryId);

        if (category is null)
        {
            return Errors.Category.NotFound;
        }

        var product = Product.Create(
            name: request.Name,
            description: request.Description,
            published: request.Published,
            brandId: brandId,
            categoryId: categoryId);

        await _productRepository.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return product;
    }
}
