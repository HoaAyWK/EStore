using EStore.Api.Controllers;
using EStore.Api.UnitTests.Controllers.Brands.TestUtils;
using EStore.Application.Brands.Commands.CreateBrand;
using EStore.Contracts.Brands;
using EStore.Domain.BrandAggregate;
using EStore.Domain.Common.Errors;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.UnitTests.Controllers.Brands.CreateBrand;

public class CreateBrandTests
{
    private readonly Mock<ISender> _mockMediator;
    private readonly Mock<IMapper> _mockMapper;
    private readonly BrandsController _controller;

    public CreateBrandTests()
    {
        _mockMediator = new();
        _mockMapper = new();
        _controller = new(_mockMediator.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateBrand_WhenHandleCreateCommandSuccess_ShouldReturnOkResultWithCreatedBrand()
    {
        var request = new CreateBrandRequest(Constants.Brand.Name);
        var command = CreateBrandCommandUtils.Create(request.Name);
        var brandResponse = BrandResponseUtils.Create(request.Name);

        ErrorOr<Brand> handleCommandResult = Brand.Create(command.Name).Value;

        _mockMapper.Setup(m => m.Map<CreateBrandCommand>(request))
            .Returns(command);

        _mockMapper.Setup(m => m.Map<BrandResponse>(handleCommandResult.Value))
            .Returns(brandResponse);

        _mockMediator.Setup(m => m.Send(command, default))
            .ReturnsAsync(handleCommandResult);

        
        var result = await _controller.CreateBrand(request);

        result.Should().BeOfType(typeof(CreatedAtActionResult));
    }

    [Fact]
    public async Task CreateBrand_WhenHandleCreateCommandFail_ShouldReturnProblemDetails()
    {
        var request = new CreateBrandRequest(Constants.Brand.InvalidName);
        var command = CreateBrandCommandUtils.Create(request.Name);

        ErrorOr<Brand> handleCommandResult = Errors.Brand.InvalidNameLength;

        _mockMapper.Setup(m => m.Map<CreateBrandCommand>(request))
            .Returns(command);

        _mockMediator.Setup(m => m.Send(command, default))
            .ReturnsAsync(handleCommandResult);

        
        var result = await _controller.CreateBrand(request);

        result.Should().BeOfType(typeof(ObjectResult));
    }
}
