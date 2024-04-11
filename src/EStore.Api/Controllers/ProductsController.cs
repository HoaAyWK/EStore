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
using EStore.Contracts.Common;
using EStore.Application.Products.Commands.AddProductVariant;
using EStore.Application.Products.Commands.UpdateProductVariant;
using EStore.Api.Common.ApiRoutes;
using EStore.Application.Products.Commands.AddProductReview;

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
        int pageSize = 5,
        string? order = null,
        string? orderBy = null)
    {
        var query = new GetProductListPagedQuery(
            searchTerm,
            page,
            pageSize,
            order,
            orderBy);

        var listPagedDtos = await _mediator.Send(query);
        var listPagedResponse = _mapper.Map<PagedList<ProductResponse>>(listPagedDtos);

        return Ok(listPagedResponse);
    }

    [AllowAnonymous]
    [HttpGet(ApiRoutes.Product.Get)]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var query = new GetProductByIdQuery(ProductId.Create(id));
        var getProductResult = await _mediator.Send(query);

        return getProductResult.Match(
            product => Ok(_mapper.Map<ProductResponse>(product)),
            Problem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductRequest request)
    {
        var command = _mapper.Map<CreateProductCommand>(request);
        var createProductResult = await _mediator.Send(command);

        return createProductResult.Match(
            product => Ok(_mapper.Map<ProductResponse>(product)),
            Problem);
    }

    [HttpPost]
    [Route(ApiRoutes.Product.AddImages)]
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

        if (addProductImageResult.IsError)
        {
            return Problem(addProductImageResult.Errors);
        }

        var query = new GetProductByIdQuery(ProductId.Create(id));
        var getProductResult = await _mediator.Send(query);

        return getProductResult.Match(Ok, Problem);
    }

    [HttpPut(ApiRoutes.Product.Update)]
    public async Task<IActionResult> UpdateProduct(
        Guid id,
        [FromBody] UpdateProductRequest request)
    {
        var command = _mapper.Map<UpdateProductCommand>((id, request));
        var updateProductResult = await _mediator.Send(command);

        if (updateProductResult.IsError)
        {
            return Problem(updateProductResult.Errors);
        }

        var query = new GetProductByIdQuery(ProductId.Create(id));
        var getProductResult = await _mediator.Send(query);

        return getProductResult.Match(
            product => Ok(_mapper.Map<ProductResponse>(product)),
            Problem);
    }

    [HttpPost(ApiRoutes.Product.AddAttribute)]
    public async Task<IActionResult> AddAttribute(
        Guid id,
        [FromBody] AddProductAttributeRequest request)
    {
        var command = new AddProductAttributeCommand(
            ProductId.Create(id),
            request.Name,
            request.CanCombine,
            request.DisplayOrder);

        var addAttributeResult = await _mediator.Send(command);

        if (addAttributeResult.IsError)
        {
            return Problem(addAttributeResult.Errors);
        }

        var query = new GetProductByIdQuery(ProductId.Create(id));
        var getProductResult = await _mediator.Send(query);

        return getProductResult.Match(
            product => Ok(_mapper.Map<ProductResponse>(product)),
            Problem);
    }


    [HttpPut]
    [Route(ApiRoutes.Product.UpdateAttribute)]
    public async Task<IActionResult> UpdateAttribute(
        Guid id,
        Guid attributeId,
        [FromBody] UpdateProductAttributeRequest request)
    {
        var command = new UpdateProductAttributeCommand(
            ProductAttributeId.Create(attributeId),
            ProductId.Create(id),
            request.Name,
            request.CanCombine,
            request.DisplayOrder,
            request.Colorable);

        var updateAttributeResult = await _mediator.Send(command);

        if (updateAttributeResult.IsError)
        {
            return Problem(updateAttributeResult.Errors);
        }

        var query = new GetProductByIdQuery(ProductId.Create(id));
        var getProductResult = await _mediator.Send(query);

        return getProductResult.Match(
            product => Ok(_mapper.Map<ProductResponse>(product)),
            Problem);
    }

    [HttpPost(ApiRoutes.Product.AddAttributeValue)]
    public async Task<IActionResult> AddAttributeValue(
        Guid id,
        Guid attributeId,
        AddProductAttributeValueRequest request)
    {
        var command = new AddProductAttributeValueCommand(
            ProductId: ProductId.Create(id),
            ProductAttributeId: ProductAttributeId.Create(attributeId),
            Name: request.Name,
            Color: request.Color,
            PriceAdjustment: request.PriceAdjustment,
            DisplayOrder: request.DisplayOrder);

        var addAttributeValueResult = await _mediator.Send(command);

        if (addAttributeValueResult.IsError)
        {
            return Problem(addAttributeValueResult.Errors);
        }

        var query = new GetProductByIdQuery(ProductId.Create(id));
        var getProductResult = await _mediator.Send(query);

        return getProductResult.Match(
            product => Ok(_mapper.Map<ProductResponse>(product)),
            Problem);
    }

    [HttpPut(ApiRoutes.Product.UpdateAttributeValue)]
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
            Color: request.Color,
            PriceAdjustment: request.PriceAdjustment,
            DisplayOrder: request.DisplayOrder);

        var updateAttributeValueResult = await _mediator.Send(command);

        if (updateAttributeValueResult.IsError)
        {
            return Problem(updateAttributeValueResult.Errors);
        }

        var query = new GetProductByIdQuery(ProductId.Create(id));
        var getProductResult = await _mediator.Send(query);

        return getProductResult.Match(
            product => Ok(_mapper.Map<ProductResponse>(product)),
            Problem);
    }

    [HttpDelete]
    [Route(ApiRoutes.Product.DeleteAttributeValue)]
    public async Task<IActionResult> DeleteVarianAttributeValue(
        [FromRoute] DeleteAttributeValueRequest request)
    {
        var command = _mapper.Map<DeleteAttributeValueCommand>(request);
        var deleteResult = await _mediator.Send(command);

        if (deleteResult.IsError)
        {
            return Problem(deleteResult.Errors);
        }

        var query = new GetProductByIdQuery(ProductId.Create(request.Id));
        var getProductResult = await _mediator.Send(query);

        return getProductResult.Match(
            product => Ok(_mapper.Map<ProductResponse>(product)),
            Problem);
    }

    [HttpPost]
    [Route(ApiRoutes.Product.AddVariant)]
    public async Task<IActionResult> AddProductVariant(Guid id, [FromBody] CreateProductVariantRequest request)
    {
        var command = _mapper.Map<AddProductVariantCommand>((id, request));
        var addProductVariantResult = await _mediator.Send(command);

        if (addProductVariantResult.IsError)
        {
            return Problem(addProductVariantResult.Errors);
        }

        var query = new GetProductByIdQuery(ProductId.Create(id));
        var getProductResult = await _mediator.Send(query);

        return getProductResult.Match(
            product => Ok(_mapper.Map<ProductResponse>(product)),
            Problem);
    }

    [HttpPut]
    [Route(ApiRoutes.Product.UpdateVariant)]
    public async Task<IActionResult> UpdateProductVariant(
        Guid id,
        Guid productVariantId,
        [FromBody] UpdateProductVariantRequest request)
    {
        var command = _mapper.Map<UpdateProductVariantCommand>((id, productVariantId, request));
        var updateProductVariantResult = await _mediator.Send(command);

        if (updateProductVariantResult.IsError)
        {
            return Problem(updateProductVariantResult.Errors);
        }

        var query = new GetProductByIdQuery(ProductId.Create(id));
        var getProductResult = await _mediator.Send(query);

        return getProductResult.Match(
            product => Ok(_mapper.Map<ProductResponse>(product)),
            Problem);
    }

    [HttpPost]
    [Route(ApiRoutes.Product.AddReview)]
    public async Task<IActionResult> AddProductReview(
        Guid id,
        [FromBody] AddProductReviewRequest request)
    {
        var command = _mapper.Map<AddProductReviewCommand>((id, request));

        var addProductReviewResult = await _mediator.Send(command);

        if (addProductReviewResult.IsError)
        {
            return Problem(addProductReviewResult.Errors);
        }

        var query = new GetProductByIdQuery(ProductId.Create(id));
        var getProductResult = await _mediator.Send(query);

        return getProductResult.Match(
            product => Ok(_mapper.Map<ProductResponse>(product)),
            Problem);
    }
}
