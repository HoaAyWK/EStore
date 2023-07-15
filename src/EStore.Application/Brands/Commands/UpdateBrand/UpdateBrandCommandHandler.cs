using ErrorOr;
using EStore.Domain.BrandAggregate.Repositories;
using EStore.Domain.Common.Errors;
using MediatR;

namespace EStore.Application.Brands.Commands.UpdateBrand;

public class UpdateBrandCommandHandler
    : IRequestHandler<UpdateBrandCommand, ErrorOr<Updated>>
{
    private readonly IBrandRepository _brandRepository;

    public UpdateBrandCommandHandler(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdateBrandCommand request,
        CancellationToken cancellationToken)
    {
        var brand = await _brandRepository.GetByIdAsync(request.Id);
    
        if (brand is null)
        {
            return Errors.Brand.NotFound;
        }

        var updateBrandResult = brand.UpdateName(request.Name);

        if (updateBrandResult.IsError)
        {
            return updateBrandResult.Errors;
        }

        return Result.Updated;
    }
}
