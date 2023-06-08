using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Catalog.BrandAggregate.Repositories;
using EStore.Domain.Catalog.ProductAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Brands.Commands.DeleteBrand;

public class DeleteBrandCommandHandler
    : IRequestHandler<DeleteBrandCommand, ErrorOr<Deleted>>
{
    private readonly IBrandRepository _brandRepository;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBrandCommandHandler(
        IBrandRepository brandRepository,
        IProductReadRepository productReadRepository,
        IUnitOfWork unitOfWork)
    {
        _brandRepository = brandRepository;
        _productReadRepository = productReadRepository;
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

        if (await _productReadRepository.AnyProductWithBrandId(request.Id))
        {
            return Errors.Brand.AlreadyContainedProducts;
        }

        _brandRepository.Delete(brand);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}
