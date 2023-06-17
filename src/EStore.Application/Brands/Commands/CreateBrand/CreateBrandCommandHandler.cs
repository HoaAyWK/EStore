using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.BrandAggregate;
using EStore.Domain.BrandAggregate.Repositories;
using MediatR;

namespace EStore.Application.Brands.Commands.CreateBrand;

public class CreateBrandCommandHandler
    : IRequestHandler<CreateBrandCommand, ErrorOr<Brand>>
{
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBrandCommandHandler(
        IBrandRepository brandRepository,
        IUnitOfWork unitOfWork)
    {
        _brandRepository = brandRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Brand>> Handle(
        CreateBrandCommand request,
        CancellationToken cancellationToken)
    {
        var brand = Brand.Create(request.Name);
        await _brandRepository.AddAsync(brand);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return brand;
    }
}
