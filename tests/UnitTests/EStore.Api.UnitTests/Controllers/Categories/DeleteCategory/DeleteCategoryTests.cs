using ErrorOr;
using EStore.Api.Controllers;
using EStore.Api.UnitTests.Controllers.Categories.TestUtils;
using EStore.Application.Categories.Commands.DeleteCategory;
using EStore.Domain.Common.Errors;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.UnitTests.Controllers.Categories.DeleteCategory;

public class DeleteCategoryTests
{
    private readonly Mock<ISender> _mockMediator;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CategoriesController _controller;

    public DeleteCategoryTests()
    {
        _mockMediator = new();
        _mockMapper = new();
        _controller = new(_mockMediator.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task DeleteCategory_WhenHandleCommandSuccess_ShouldReturnNoContentResult()
    {
        var command = DeleteCategoryCommandUtils.Create();
        ErrorOr<Deleted> handleCommandResult = Result.Deleted;

        _mockMapper.Setup(m => m.Map<DeleteCategoryCommand>(Constants.Category.Id.Value))
            .Returns(command);

        _mockMediator.Setup(m => m.Send(command, default))
            .ReturnsAsync(handleCommandResult);

        var result = await _controller.DeleteCategory(Constants.Category.Id.Value);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteCategory_WhenHandleCommandFail_ShouldReturnProblemDetails()
    {
        var command = DeleteCategoryCommandUtils.Create();
        ErrorOr<Deleted> handleCommandResult = Errors.Category.NotFound;

        _mockMapper.Setup(m => m.Map<DeleteCategoryCommand>(Constants.Category.Id.Value))
            .Returns(command);

        _mockMediator.Setup(m => m.Send(command, default))
            .ReturnsAsync(handleCommandResult);

        var result = await _controller.DeleteCategory(Constants.Category.Id.Value);

        result.Should().BeOfType<ObjectResult>();
    }
}
