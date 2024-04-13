using ErrorOr;
using EStore.Api.Controllers;
using EStore.Api.UnitTests.Controllers.Categories.TestUtils;
using EStore.Application.Categories.Commands.CreateCategory;
using EStore.Contracts.Categories;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.Common.Errors;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.UnitTests.Controllers.Categories.CreateCategory;

public class CreateCategoryTests
{
    private readonly Mock<ISender> _mockMediator;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CategoriesController _controller;

    public CreateCategoryTests()
    {
        _mockMediator = new();
        _mockMapper = new();
        _controller = new(_mockMediator.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateCategory_WhenHandleCreateCommandSuccess_ShouldReturnCreatedAtActionResult()
    {
        var request = CreateCategoryRequestUtils.CreateRequest(Constants.Category.Name, null);

        var command = CreateCategoryCommandUtils.CreateCommand(
            request.Name,
            request.Slug,
            request.ImageUrl,
            null);

        ErrorOr<Category> handleCommandResult = Category.Create(
            command.Name,
            command.Slug,
            command.ImageUrl,
            command.ParentId);

        _mockMapper.Setup(m => m.Map<CreateCategoryCommand>(request))
            .Returns(command);

        _mockMediator.Setup(m => m.Send(command, default))
            .ReturnsAsync(handleCommandResult);

        _mockMapper.Setup(m => m.Map<CategoryResponse>(handleCommandResult.Value))
            .Returns(CategoryResponseUtils.Create(
                handleCommandResult.Value.Id.Value,
                handleCommandResult.Value.Name,
                null));

        var result = await _controller.CreateCategory(request);

        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task CreateCategory_WhenHandleCommandFail_ShouldReturnProblemDetails()
    {
        var request = CreateCategoryRequestUtils.CreateRequest(Constants.Category.Name, null);
        var command = CreateCategoryCommandUtils.CreateCommand(
            request.Name,
            request.Slug,
            request.ImageUrl,
            null);

        ErrorOr<Category> handleCommandResult = Errors.Category.InvalidNameLength;

        _mockMapper.Setup(m => m.Map<CreateCategoryCommand>(request))
            .Returns(command);

        _mockMediator.Setup(m => m.Send(command, default))
            .ReturnsAsync(handleCommandResult);

        var result = await _controller.CreateCategory(request);

        result.Should().BeOfType(typeof(ObjectResult));
    }

    [Fact]
    public async Task CreateCategory_WhenParentCannotParse_ShouldReturnProblemDetails()
    {
        var request = CreateCategoryRequestUtils.CreateRequest(
            Constants.Category.Name,
            Constants.Category.InvalidParentId);

        var result = await _controller.CreateCategory(request);

        result.Should().BeOfType(typeof(ObjectResult));
    }
}