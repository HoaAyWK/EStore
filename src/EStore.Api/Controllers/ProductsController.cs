using EStore.Application.Products.Commands.AddProductAttributeValue;
using EStore.Application.Products.Commands.AddProductImage;
using EStore.Application.Products.Commands.CreateProduct;
using EStore.Application.Products.Commands.DeleteAttributeValue;
using EStore.Application.Products.Commands.UpdateProduct;
using EStore.Application.Products.Commands.AddProductAttribute;
using EStore.Application.Products.Commands.UpdateProductAttribute;
using EStore.Application.Products.Commands.UpdateProductAttributeValue;
using EStore.Application.Products.Queries.GetProductById;
using EStore.Application.Products.Queries.GetProductListPaged;
using EStore.Contracts.Products;
using EStore.Domain.ProductAggregate.ValueObjects;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EStore.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Contracts.Common;

namespace EStore.Api.Controllers;

[Authorize(Roles = $"{Roles.Admin}")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProductsController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public ProductsController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetProducts(
        string? searchTerm,
        int page = 1,
        int pageSize = 5)
    {
        var query = new GetProductListPagedQuery(
            searchTerm,
            page,
            pageSize);

        var listPagedDtos = await _mediator.Send(query);
        var listPagedResponse = _mapper.Map<PagedList<ProductResponse>>(listPagedDtos);

        return Ok(listPagedResponse);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var query = new GetProductByIdQuery(ProductId.Create(id));
        var getProductResult = await _mediator.Send(query);

        return getProductResult.Match(
            product => Ok(_mapper.Map<ProductResponse>(product)),
            errors => Problem(errors));
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductRequest request)
    {
        var command = _mapper.Map<CreateProductCommand>(request);
        var createProductResult = await _mediator.Send(command);

        return createProductResult.Match(
            product => Ok(_mapper.Map<ProductResponse>(product)),
            errors => Problem(errors));
    }

    [HttpPost]
    [Route("{id:guid}/images")]
    public async Task<IActionResult> AddProductImage(
        Guid id,
        [FromBody] AddProductImageRequest request)
    {
        var command = new AddProductImageCommand(
            ProductId: ProductId.Create(id),
            ImageUrl: request.ImageUrl,
            IsMain: request.IsMain,
            DisplayOrder: request.DisplayOrder);

        var addProductImageResult = await _mediator.Send(command);

        return addProductImageResult.Match(
            updated => NoContent(),
            errors => Problem(errors));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProduct(
        Guid id,
        [FromBody] UpdateProductRequest request)
    {
        var command = new UpdateProductCommand(
            Id: ProductId.Create(id),
            Name: request.Name,
            Description: request.Description,
            Price: request.Price,
            Published: request.Published,
            StockQuantity: request.StockQuantity,
            DisplayOrder: request.DisplayOrder,
            BrandId: _mapper.Map<BrandId>(request.BrandId),
            CategoryId: _mapper.Map<CategoryId>(request.CategoryId),
            SpecialPrice: request.SpecialPrice,
            SpecialPriceStartDate: request.SpecialPriceStartDate,
            SpecialPriceEndDate: request.SpecialPriceEndDate);

        var updateProductResult = await _mediator.Send(command);

        return updateProductResult.Match(
            product => Ok(_mapper.Map<ProductResponse>(product)),
            errors => Problem(errors));
    }

    [HttpPost("{id:guid}/attributes")]
    public async Task<IActionResult> AddAttribute(
        Guid id,
        [FromBody] AddProductAttributeRequest request)
    {
        var command = new AddProductAttributeCommand(
            ProductId.Create(id),
            request.Name,
            request.Alias,
            request.CanCombine);

        var addAttributeResult = await _mediator.Send(command);

        return addAttributeResult.Match(
            result => Ok(result),
            errors => Problem(errors));
    }


    [HttpPut]
    [Route("{id:guid}/attributes/{attributeId:guid}")]
    public async Task<IActionResult> UpdateAttribute(
        Guid id,
        Guid attributeId,
        [FromBody] UpdateProductAttributeRequest request)
    {
        var command = new UpdateProductAttributeCommand(
            ProductAttributeId.Create(attributeId),
            ProductId.Create(id),
            request.Name,
            request.Alias,
            request.CanCombine);

        var updateAttributeResult = await _mediator.Send(command);

        return updateAttributeResult.Match(
            result => Ok(result),
            errors => Problem(errors));
    }

    [HttpPost("{id:guid}/attributes/{attributeId:guid}/values")]
    public async Task<IActionResult> AddAttributeValue(
        Guid id,
        Guid attributeId,
        AddProductAttributeValueRequest request)
    {
        var command = new AddProductAttributeValueCommand(
            ProductId: ProductId.Create(id),
            ProductAttributeId: ProductAttributeId.Create(attributeId),
            Name: request.Name,
            Alias: request.Alias,
            PriceAdjustment: request.PriceAdjustment);

        var addAttributeValueResult = await _mediator.Send(command);

        return addAttributeValueResult.Match(
            addResult => Ok(addResult),
            errors => Problem(errors));
    }

    [HttpPut("{id:guid}/attributes/{attributeId:guid}/values/{attributeValueId:guid}")]
    public async Task<IActionResult> UpdateAttributeValue(
        Guid id,
        Guid attributeId,
        Guid attributeValueId,
        [FromBody] UpdateProductAttributeValueRequest request)
    {
        var command = new UpdateProductAttributeValueCommand(
            ProductAttributeValueId: ProductAttributeValueId.Create(attributeValueId),
            ProductId: ProductId.Create(id),
            ProductAttributeId: ProductAttributeId.Create(attributeId),
            Name: request.Name,
            Alias: request.Alias,
            PriceAdjustment: request.PriceAdjustment);

        var updateAttributeValueResult = await _mediator.Send(command);

        return updateAttributeValueResult.Match(
            updated => NoContent(),
            errors => Problem(errors));
    }

    [HttpDelete]
    [Route("{id:guid}/attributes/{attributeId:guid}/values/{attributeValueId:guid}")]
    public async Task<IActionResult> DeleteVarianAttributeValue(
        [FromRoute] DeleteAttributeValueRequest request)
    {
        var command = _mapper.Map<DeleteAttributeValueCommand>(request);
        var deleteResult = await _mediator.Send(command);

        return deleteResult.Match(
            deleteResult => Ok(deleteResult),
            errors => Problem(errors));
    }
}
