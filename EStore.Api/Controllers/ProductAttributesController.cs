using EStore.Application.ProductAttributes.Commands.AddProductAttributeOption;
using EStore.Application.ProductAttributes.Commands.AddProductAttributeOptionSet;
using EStore.Application.ProductAttributes.Commands.CreateProductAttribute;
using EStore.Application.ProductAttributes.Commands.DeleteProductAttribute;
using EStore.Application.ProductAttributes.Commands.UpdateProductAttribute;
using EStore.Application.ProductAttributes.Queries.GetProductAttributeById;
using EStore.Application.ProductAttributes.Queries.GetProductAttributeListPaged;
using EStore.Contracts.ProductAttributes;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

[Route("api/product-attributes")]
public class ProductAttributesController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public ProductAttributesController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductAttributes()
    {
        var getProductAttributesResult = await _mediator.Send(
            new GetProductAttributeListPagedQuery());
        
        return getProductAttributesResult.Match(
            productAttributes => Ok(productAttributes),
            errors => Problem(errors));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductAttribute(string id)
    {
        var query = new GetProductAttributeByIdQuery(id);
        var getProductAttributeResult = await _mediator.Send(query);

        return getProductAttributeResult.Match(
            productAttribute => Ok(productAttribute),
            errors => Problem(errors));
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAttribute(
        CreateProductAttributeRequest request)
    {
        var command = _mapper.Map<CreateProductAttributeCommand>(request);
        var createProductAttributeResult = await _mediator.Send(command);

        return createProductAttributeResult.Match(
            productAttribute => Ok(productAttribute),
            errors => Problem(errors));
    }

    [HttpPost]
    [Route("option-sets")]
    public async Task<IActionResult> AddOptionSet(
        AddProductAttributeOptionSetRequest request)
    {
        var command = _mapper.Map<AddProductAttributeOptionSetCommand>(request);
        var addOptionSetResult = await _mediator.Send(command);

        return addOptionSetResult.Match(
            optionSet => Ok(optionSet),
            errors => Problem(errors));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProductAttribute(
        UpdateProductAttributeRequest request)
    {
        var command = _mapper.Map<UpdateProductAttributeCommand>(request);
        var updateProductAttributeResult = await _mediator.Send(command);

        return updateProductAttributeResult.Match(
            productAttribute => Ok(productAttribute),
            errors => Problem(errors));
    }

    [HttpPost]
    [Route("add-option")]
    public async Task<IActionResult> AddOption(AddProductAttributeOptionRequest request)
    {
        var command = _mapper.Map<AddProductAttributeOptionCommand>(request);

        var addOptionResult = await _mediator.Send(command);

        return addOptionResult.Match(
            option => Ok(option),
            errors => Problem(errors));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductAttribute(string id)
    {
        var deleteProductAttributeResult = await _mediator.Send(
            new DeleteProductAttributeCommand(id));

        return deleteProductAttributeResult.Match(
            deleteResult => Ok(deleteResult),
            errors => Problem(errors));
    }
}
