using ErrorOr;
using EStore.Domain.BrandAggregate;
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
        var createBrandResult = Brand.Create(request.Name);

        if (createBrandResult.IsError)
        {
            return createBrandResult.Errors;
        }

        var brand = createBrandResult.Value;

        await _brandRepository.AddAsync(brand);

        return brand;
    }
}
