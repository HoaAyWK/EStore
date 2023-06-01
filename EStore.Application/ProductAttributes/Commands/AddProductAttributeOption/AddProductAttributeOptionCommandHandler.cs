using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Common.Errors;
using EStore.Domain.Catalog.ProductAttributeAggregate;
using EStore.Domain.Catalog.ProductAttributeAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.ProductAttributes.Commands.AddProductAttributeOption;

public class AddProductAttributeOptionCommandHandler
    : IRequestHandler<AddProductAttributeOptionCommand, ErrorOr<ProductAttributeOption>>
{
    private readonly IProductAttributeReadRepository _productAttributeReadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddProductAttributeOptionCommandHandler(
        IProductAttributeReadRepository productAttributeReadRepository,
        IUnitOfWork unitOfWork)
    {
        _productAttributeReadRepository = productAttributeReadRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<ProductAttributeOption>> Handle(
        AddProductAttributeOptionCommand request,
        CancellationToken cancellationToken)
    {
        var productAttribute = await _productAttributeReadRepository.GetByIdAsync(
            ProductAttributeId.Create(new Guid(request.ProductAttributeId)));

        if (productAttribute is null)
        {
            return Errors.ProductAttribute.NotFound;
        }

        ProductAttributeOptionSetId optionSetId = ProductAttributeOptionSetId.Create(
            new Guid(request.ProductAttributeOptionSetId));

        var optionSet = productAttribute.ProductAttributeOptionSets
            .FirstOrDefault(x => x.Id == optionSetId);

        if (optionSet is null)
        {
            return Errors.ProductAttribute.OptionSetNotFound;
        }

        var productAttributeOption = ProductAttributeOption.Create(
            name: request.Name,
            priceAdjustment: request.PriceAdjustment ?? 0,
            alias: request.Alias);

        optionSet.AddOption(productAttributeOption);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return productAttributeOption;
    }
}
