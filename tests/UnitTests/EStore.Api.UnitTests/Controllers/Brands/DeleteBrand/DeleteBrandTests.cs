using System.Collections.Generic;
using ErrorOr;
using EStore.Api.Controllers;
using EStore.Api.UnitTests.Controllers.Brands.TestUtils;
using EStore.Application.Brands.Commands.DeleteBrand;
using EStore.Domain.Common.Errors;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.UnitTests.Controllers.Brands.DeleteBrand;

public class DeleteBrandTests
{
    private readonly Mock<ISender> _mockMediator;
    private readonly Mock<IMapper> _mockMapper;
    private readonly BrandsController _controller;

    public DeleteBrandTests()
    {
        _mockMediator = new();
        _mockMapper = new();
        _controller = new(_mockMediator.Object, _mockMapper.Object);
    }
    
    [Fact]
    public async Task DeleteBrand_WhenHandleDeleteCommandSuccess_ShouldReturnNoContentResult()
    {
        var command = DeleteBrandCommandUtils.Create();

        ErrorOr<Deleted> handleCommandResult = Result.Deleted;

        _mockMediator.Setup(m => m.Send(command, default))
            .ReturnsAsync(handleCommandResult);

        var result = await _controller.DeleteBrand(Constants.Brand.Id.Value);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteBrand_WhenHandleDeleteCommandFail_ShouldReturnProblemDetails()
    {
        var command = DeleteBrandCommandUtils.Create();
        ErrorOr<Deleted> handleCommandResult = Errors.Brand.NotFound;

        _mockMapper.Setup(m => m.Map<DeleteBrandCommand>(Constants.Brand.Id.Value))
            .Returns(command);

        _mockMediator.Setup(m => m.Send(command, default))
            .ReturnsAsync(handleCommandResult);

        var result = await _controller.DeleteBrand(Constants.Brand.Id.Value);

        result.Should().BeOfType<ObjectResult>();
    }
}
