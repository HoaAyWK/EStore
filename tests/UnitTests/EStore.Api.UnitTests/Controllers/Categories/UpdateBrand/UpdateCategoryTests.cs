using System;
using ErrorOr;
using EStore.Api.Controllers;
using EStore.Api.UnitTests.Controllers.Categories.TestUtils;
using EStore.Application.Categories.Commands.UpdateCategory;
using EStore.Contracts.Categories;
using EStore.Domain.Common.Errors;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.UnitTests.Controllers.Categories.UpdateCategory;

public class UpdateCategoryTests
{
    private readonly Mock<ISender> _mockMediator;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CategoriesController _controller;

    public UpdateCategoryTests()
    {
        _mockMediator = new();
        _mockMapper = new();
        _controller = new(_mockMediator.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task UpdateCategory_WhenHandleCommandSuccess_ShouldReturnNoContentResult()
    {
        var request = UpdateCategoryRequestUtils.Create(
            Constants.Category.Name,
            Constants.Category.Name,
            string.Empty,
            null);

        var command = UpdateCategoryCommandUtils.Create(
            request.Name,
            request.Slug,
            request.ImageUrl);

        (Guid, UpdateCategoryRequest) src = (Constants.Category.Id.Value, request);
        ErrorOr<Updated> handleCommandResult = Result.Updated;

        _mockMapper.Setup(m => m.Map<UpdateCategoryCommand>(src))
            .Returns(command);

        _mockMediator.Setup(m => m.Send(command, default))
            .ReturnsAsync(handleCommandResult);

        var result = await _controller.UpdateCategory(Constants.Category.Id.Value, request);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task UpdateCategory_WhenHandleCommandFail_ShouldReturnProblemDetails()
    {
        var request = UpdateCategoryRequestUtils.Create(
            Constants.Category.NameUnderMinLength,
            Constants.Category.Name,
            string.Empty,
            null);

        var command = UpdateCategoryCommandUtils.Create(
            request.Name,
            request.Slug,
            request.ImageUrl);
        
        (Guid, UpdateCategoryRequest) src = (Constants.Category.Id.Value, request);
        ErrorOr<Updated> handleCommandResult = Errors.Category.InvalidNameLength;

        _mockMapper.Setup(m => m.Map<UpdateCategoryCommand>(src))
            .Returns(command);

        _mockMediator.Setup(m => m.Send(command, default))
            .ReturnsAsync(handleCommandResult);

        var result = await _controller.UpdateCategory(Constants.Category.Id.Value, request);

        result.Should().BeOfType<ObjectResult>();
    }

    [Fact]
    public async Task UpdateCategory_WhenParentIdCannotParse_ShouldReturnProblemDetails()
    {
        var request = UpdateCategoryRequestUtils.Create(
            Constants.Category.Name,
            Constants.Category.Name,
            string.Empty,
            Constants.Category.InvalidParentId);

        var result = await _controller.UpdateCategory(Constants.Category.Id.Value, request);

        result.Should().BeOfType<ObjectResult>();
    }
}

