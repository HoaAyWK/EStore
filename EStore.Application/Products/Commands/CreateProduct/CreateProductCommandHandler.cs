using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Catalog.BrandAggregate;
using EStore.Domain.Catalog.BrandAggregate.Repositories;
using EStore.Domain.Catalog.BrandAggregate.ValueObjects;
using EStore.Domain.Catalog.CategoryAggregate;
using EStore.Domain.Catalog.CategoryAggregate.Repositories;
using EStore.Domain.Catalog.CategoryAggregate.ValueObjects;
using EStore.Domain.Catalog.ProductAggregate;
using EStore.Domain.Catalog.ProductAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;
    private readonly IBrandReadRepository _brandReadRepository;
    private readonly ICategoryReadRepository _categoryReadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IBrandReadRepository brandReadRepository,
        ICategoryReadRepository categoryReadRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _brandReadRepository = brandReadRepository;
        _categoryReadRepository = categoryReadRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Product>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        BrandId brandId = BrandId.Create(new Guid(request.BrandId));
        var brand = await _brandReadRepository.GetByIdAsync(brandId);

        if (brand is null)
        {
            return Errors.Brand.NotFound;
        }

        CategoryId categoryId = CategoryId.Create(new Guid(request.CategoryId));
        var category = await _categoryReadRepository.GetByIdAsync(categoryId);

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
