using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
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
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IBrandRepository brandRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _brandRepository = brandRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
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
            displayOrder: request.DisplayOrder);
        
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

        product.UpdatePublished(request.Published);

        if (request.SpecialPrice is not null)
        {
            if (request.SpecialPriceStartDate is null)
            {
                errors.Add(Errors.Product.UnprovidedSpecialPriceStartDate);
            }

            if (request.SpecialPriceEndDate is null)
            {
                errors.Add(Errors.Product.UnprovidedSpecialPriceEndDate);
            }

            var updateSpecialPriceResult =  product.UpdateSpecialPrice(
                specialPrice: request.SpecialPrice.Value,
                startDate: request.SpecialPriceStartDate!.Value,
                endDate: request.SpecialPriceEndDate!.Value);

            if (updateDetailsResult.IsError)
            {
                errors.AddRange(updateDetailsResult.Errors);
            }
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product;
    }
}
