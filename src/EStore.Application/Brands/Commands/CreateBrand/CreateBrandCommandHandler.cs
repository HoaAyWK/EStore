using ErrorOr;
using EStore.Domain.BrandAggregate;
using EStore.Domain.Common.Errors;
using EStore.Domain.BrandAggregate.Repositories;
using MediatR;

namespace EStore.Application.Brands.Commands.CreateBrand;

public class CreateBrandCommandHandler
    : IRequestHandler<CreateBrandCommand, ErrorOr<Brand>>
{
    private readonly IBrandRepository _brandRepository;

    public CreateBrandCommandHandler(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task<ErrorOr<Brand>> Handle(
        CreateBrandCommand request,
        CancellationToken cancellationToken)
    {
        var existingBrand = await _brandRepository.GetByNameAsync(request.Name);

        if (existingBrand is not null)
        {
            return Errors.Brand.BrandAlreadyExists(request.Name);
        }

        var createBrandResult = Brand.Create(request.Name, request.ImageUrl);

        if (createBrandResult.IsError)
        {
            return createBrandResult.Errors;
        }

        var brand = createBrandResult.Value;

        await _brandRepository.AddAsync(brand);

        return brand;
    }
}
