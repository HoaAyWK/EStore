using EStore.Api.Controllers;
using EStore.Api.UnitTests.Controllers.Brands.TestUtils;
using EStore.Domain.Common.Errors;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using EStore.Application.Brands.Commands.UpdateBrand;
using EStore.Contracts.Brands;
using System;

namespace EStore.Api.UnitTests.Controllers.Brands.UpdateBrand;

public class UpdateBrandTests
{
    private readonly Mock<ISender> _mockMediator;
    private readonly Mock<IMapper> _mockMapper;
    private readonly BrandsController _controller;

    public UpdateBrandTests()
    {
        _mockMediator = new();
        _mockMapper = new();
        _controller = new(_mockMediator.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task UpdateBrand_WhenHandleUpdateCommandSuccess_ShouldReturnNoContentResult()
    {
        var request = UpdateBrandRequestUtils.Create();
        var command = UpdateBrandCommandUtils.Create();
        (Guid, UpdateBrandRequest) src = (Constants.Brand.Id.Value, request);
        ErrorOr<Updated> handleCommandResult = Result.Updated;

        _mockMapper.Setup(m => m.Map<UpdateBrandCommand>(src))
            .Returns(command);

        _mockMediator.Setup(m => m.Send(command, default))
            .ReturnsAsync(handleCommandResult);
        
        var result = await _controller.UpdateBrand(Constants.Brand.Id.Value, request);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task UpdateBrand_WhenHandleUpdateCommandFail_ShouldReturnProblemDetails()
    {
        var request = UpdateBrandRequestUtils.Create();
        var command = UpdateBrandCommandUtils.Create();
        (Guid, UpdateBrandRequest) src = (Constants.Brand.Id.Value, request);
        ErrorOr<Updated> handleCommandResult = Errors.Brand.InvalidNameLength;

        _mockMapper.Setup(m => m.Map<UpdateBrandCommand>(src))
            .Returns(command);

        _mockMediator.Setup(m => m.Send(command, default))
            .ReturnsAsync(handleCommandResult);

        
        var result = await _controller.UpdateBrand(Constants.Brand.Id.Value, request);

        result.Should().BeOfType<ObjectResult>();
    }
}
