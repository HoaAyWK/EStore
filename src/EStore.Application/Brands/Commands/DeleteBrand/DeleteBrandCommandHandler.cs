using ErrorOr;
using EStore.Domain.BrandAggregate.Repositories;
using EStore.Domain.ProductAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Brands.Commands.DeleteBrand;

public class DeleteBrandCommandHandler
    : IRequestHandler<DeleteBrandCommand, ErrorOr<Deleted>>
{
    private readonly IBrandRepository _brandRepository;
    private readonly IProductRepository _productRepository;

    public DeleteBrandCommandHandler(
        IBrandRepository brandRepository,
        IProductRepository productRepository)
    {
        _brandRepository = brandRepository;
        _productRepository = productRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(
        DeleteBrandCommand request,
        CancellationToken cancellationToken)
    {
        var brand = await _brandRepository.GetByIdAsync(request.Id);

        if (brand is null)
        {
            return Errors.Brand.NotFound;
        }

        if (await _productRepository.AnyProductWithBrandId(request.Id))
        {
            return Errors.Brand.AlreadyContainedProducts;
        }

        _brandRepository.Delete(brand);

        return Result.Deleted;
    }
}
