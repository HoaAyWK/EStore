using ErrorOr;
using EStore.Domain.ProductAggregate;
using EStore.Domain.Common.Errors;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.BrandAggregate.Repositories;
using EStore.Domain.CategoryAggregate.Repositories;
using MediatR;

namespace EStore.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IBrandRepository brandRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _brandRepository = brandRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ErrorOr<Product>> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);

        // Return a not found status when product is not found.
        if (product is null)
        {
            return Errors.Product.NotFound;
        }

        List<Error> errors = new();

        if (request.CategoryId != product.BrandId)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

            if (category is null)
            {
                errors.Add(Errors.Category.NotFoundValidation(request.CategoryId));
            }

            product.UpdateCategory(request.CategoryId);
        }

        if (request.BrandId != product.BrandId)
        {
            var brand = await _brandRepository.GetByIdAsync(request.BrandId);

            if (brand is null)
            {
                errors.Add(Errors.Brand.NotFoundValidation(request.BrandId));
            }

            product.UpdateBrand(request.BrandId);
        }

        var updateDetailsResult = product.UpdateDetails(
            name: request.Name,
            description: request.Description,
            price: request.Price,
            displayOrder: request.DisplayOrder,
            published: request.Published,
            specialPrice: request.SpecialPrice,
            specialPriceStartDate: request.SpecialPriceStartDate,
            specialPriceEndDate: request.SpecialPriceEndDate);
        
        if (updateDetailsResult.IsError)
        {
            errors.AddRange(updateDetailsResult.Errors);
        }

        if (request.StockQuantity is not null)
        {
            var updateStockQuantityResult = product.UpdateStockQuantity(request.StockQuantity.Value);

            if (updateStockQuantityResult.IsError)
            {
                errors.Add(updateDetailsResult.FirstError);
            }
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return product;
    }
}
