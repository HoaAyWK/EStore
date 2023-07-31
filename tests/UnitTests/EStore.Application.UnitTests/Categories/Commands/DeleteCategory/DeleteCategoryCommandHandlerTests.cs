using ErrorOr;
using EStore.Application.Categories.Commands.DeleteCategory;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Application.UnitTests.Categories.TestUtils;
using EStore.Application.UnitTests.TestUtils.Constants;
using EStore.Domain.CategoryAggregate.Repositories;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.Common.Errors;
using EStore.Domain.ProductAggregate.Repositories;
using Moq;

namespace EStore.Application.UnitTests.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandlerTests
{
    private DeleteCategoryCommandHandler _handler;
    private Mock<ICategoryRepository> _mockCategoryRepository;
    private Mock<IProductRepository> _mockProductRepository;
    private Mock<IUnitOfWork> _mockUnitOfWork;

    public DeleteCategoryCommandHandlerTests()
    {
        _mockCategoryRepository = new();
        _mockUnitOfWork = new();
        _mockProductRepository = new();

        _handler = new(
            _mockCategoryRepository.Object,
            _mockProductRepository.Object);

        _mockCategoryRepository.Setup(m => m.GetByIdAsync(Constants.Category.Id))
            .ReturnsAsync(Constants.Category.MockExistingCategoryNoParent);

        Constants.Category.MockExistingCategoryHasParent
            .AddChildren(Constants.Category.MockExistingCategoryNoParent);

        _mockCategoryRepository.Setup(m => m.GetByIdAsync(Constants.Category.OtherId))
            .ReturnsAsync(Constants.Category.MockExistingCategoryHasParent);
    }

    [Fact]
    public async Task HandleDeleteCategoryCommand_WhenCategoryIsValid_ShouldDeleteCategory()
    {
        var deleteCategoryCommand = DeleteCategoryCommandUtils
            .CreateCommand(Constants.Category.Id);

        var result = await _handler.Handle(deleteCategoryCommand, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Deleted);

        _mockCategoryRepository.Verify(m => m.GetByIdAsync(deleteCategoryCommand.Id), Times.Once);
        _mockProductRepository.Verify(m => m.AnyProductWithCategoryId(deleteCategoryCommand.Id), Times.Once);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }


    [Fact]
    public async Task HandleDeleteCategoryCommand_WhenCategoryIsNotFound_ShouldReturnCategoryNotFoundError()
    {
        var deleteCategoryCommand = DeleteCategoryCommandUtils
            .CreateCommand(CategoryId.CreateUnique());

        var result = await _handler.Handle(deleteCategoryCommand, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Category.NotFound);

        _mockCategoryRepository.Verify(m => m.GetByIdAsync(deleteCategoryCommand.Id), Times.Once);
        _mockProductRepository.Verify(m => m.AnyProductWithCategoryId(deleteCategoryCommand.Id), Times.Never);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task HandleDeleteCategoryCommand_WhenCategoryIsFoundAndCategoryHasChildren_ShouldReturnCategoryAlreadyContainedChildrenError()
    {
        var deleteCategoryCommand = DeleteCategoryCommandUtils
            .CreateCommand(Constants.Category.OtherId);

        var result = await _handler.Handle(deleteCategoryCommand, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Category.AlreadyContainedChildren);

        _mockCategoryRepository.Verify(m => m.GetByIdAsync(deleteCategoryCommand.Id), Times.Once);
        _mockProductRepository.Verify(m => m.AnyProductWithCategoryId(deleteCategoryCommand.Id), Times.Never);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(default), Times.Never);
    }

    // TODO: handle category has products.
}
