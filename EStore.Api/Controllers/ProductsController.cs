using EStore.Application.Products.Commands.AddProductImage;
using EStore.Application.Products.Commands.CreateProduct;
using EStore.Application.Products.Commands.UpdateProduct;
using EStore.Application.Products.Queries.GetProductById;
using EStore.Contracts.Products;
using EStore.Domain.Catalog.ProductAggregate.ValueObjects;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

public class ProductsController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public ProductsController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var query = new GetProductByIdQuery(ProductId.Create(id));
        var getProductResult = await _mediator.Send(query);

        return getProductResult.Match(
            product => Ok(product),
            errors => Problem(errors));
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductRequest request)
    {
        var command = _mapper.Map<CreateProductCommand>(request);
        var createProductResult = await _mediator.Send(command);

        return createProductResult.Match(
            product => Ok(product),
            errors => Problem(errors));
    }

    [HttpPost]
    [Route("images")]
    public async Task<IActionResult> AddProductImage(AddProductImageRequest request)
    {
        var command = _mapper.Map<AddProductImageCommand>(request);
        var addProductImageResult = await _mediator.Send(command);

        return addProductImageResult.Match(
            product => Ok(product),
            errors => Problem(errors));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct(UpdateProductRequest request)
    {
        var command = _mapper.Map<UpdateProductCommand>(request);
        var updateProductResult = await _mediator.Send(command);

        return updateProductResult.Match(
            product => Ok(product),
            errors => Problem(errors));
    }
}
