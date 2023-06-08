using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Catalog.ProductAggregate;
using EStore.Domain.Common.Errors;
using EStore.Domain.Catalog.ProductAggregate.ValueObjects;
using EStore.Domain.Catalog.BrandAggregate.ValueObjects;
using EStore.Domain.Catalog.CategoryAggregate.ValueObjects;
using EStore.Domain.Catalog.ProductAggregate.Repositories;
using EStore.Domain.Catalog.BrandAggregate.Repositories;
using EStore.Domain.Catalog.CategoryAggregate.Repositories;
using MediatR;

namespace EStore.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;
    private readonly IBrandReadRepository _brandReadRepository;
    private readonly ICategoryReadRepository _categoryReadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(
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
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);

        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        BrandId brandId = BrandId.Create(new Guid(request.BrandId));

        if (product.BrandId != brandId)
        {
            var brand = _brandReadRepository.GetByIdAsync(brandId);

            if (brand is null)
            {
                return Errors.Brand.NotFound;
            }

            product.UpdateBrand(brandId);
        }

        CategoryId categoryId = CategoryId.Create(new Guid(request.CategoryId));

        if (product.CategoryId != categoryId)
        {
            var category = _categoryReadRepository.GetByIdAsync(categoryId);

            if (category is null)
            {
                return Errors.Category.NotFound;
            }

            product.UpdateCategory(categoryId);
        }

        product.UpdateDetails(
            name: request.Name,
            description: request.Description,
            price: request.Price);

        product.UpdatePublished(request.Published);

        if (request.SpecialPrice is not null)
        {
            if (request.SpecialPriceStartDate is null)
            {
                return Errors.Product.UnprovidedSpecialPriceStartDate;
            }

            if (request.SpecialPriceEndDate is null)
            {
                return Errors.Product.UnprovidedSpecialPriceEndDate;
            }

            if (request.SpecialPriceStartDate <= request.SpecialPriceEndDate)
            {
                return Errors.Product.SpecialPriceStartDateLessThanEndDate;
            }

            product.UpdateSpecialPrice(
                specialPrice: request.SpecialPrice.Value,
                startDate: request.SpecialPriceStartDate.Value,
                endDate: request.SpecialPriceEndDate.Value);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product;
    }
}
