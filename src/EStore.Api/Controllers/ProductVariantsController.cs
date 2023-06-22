using EStore.Application.ProductVariants.Commands.CreateProductVariant;
using EStore.Application.ProductVariants.Commands.UpdateProductVariant;
using EStore.Contracts.ProductVariants;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

[Route("/api/product-variants")]
public class ProductVariantsController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public ProductVariantsController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductVariant(CreateProductVariantRequest request)
    {
        var command = _mapper.Map<CreateProductVariantCommand>(request);

        var createProductVariantResult = await _mediator.Send(command);

        return createProductVariantResult.Match(
            productVariant => Ok(productVariant),
            errors => Problem(errors));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProductVariant(Guid id, [FromBody] UpdateProductVariantRequest request)
    {
        var command = _mapper.Map<UpdateProductVariantCommand>((id, request));
        var updateProductVariantResult = await _mediator.Send(command);

        return updateProductVariantResult.Match(
            updated => NoContent(),
            errors => Problem(errors));
    }
}
