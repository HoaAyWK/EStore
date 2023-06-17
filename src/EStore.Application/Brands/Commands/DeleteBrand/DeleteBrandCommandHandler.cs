using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
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
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBrandCommandHandler(
        IBrandRepository brandRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _brandRepository = brandRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
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
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}
