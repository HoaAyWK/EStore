using ErrorOr;
using EStore.Application.Categories.Commands.UpdateCategory;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Application.UnitTests.Categories.TestUtils;
using EStore.Application.UnitTests.TestUtils.Constants;
using EStore.Domain.CategoryAggregate.Repositories;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.Common.Errors;
using Moq;

namespace EStore.Application.UnitTests.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandlerTests
{
    private readonly UpdateCategoryCommandHandler _handler;
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public UpdateCategoryCommandHandlerTests()
    {
        _mockCategoryRepository = new();
        _mockUnitOfWork = new();
        _handler = new(_mockCategoryRepository.Object, _mockUnitOfWork.Object);

        _mockCategoryRepository.Setup(m => m.GetByIdAsync(Constants.Category.Id))
            .ReturnsAsync(Constants.Category.MockExistingCategoryNoParent);

        _mockCategoryRepository.Setup(m => m.GetByIdAsync(Constants.Category.ParentId))
            .ReturnsAsync(Constants.Category.MockExistingParentCategory);
    }

    [Theory]
    [ClassData(typeof(UpdateCategoryCommandWhenCategoryNameAndParentIdIsValidData))]
    public async Task HandleUpdateCategoryCommand_WhenCategoryNameAndParentIdIsValid_ShouldUpdateCategory(UpdateCategoryCommand command)
    {
        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Updated);

        _mockCategoryRepository.Verify(m => m.GetByIdAsync(command.Id), Times.Once);
        _mockCategoryRepository.Verify(m => m.GetByIdAsync(command.ParentId!), Times.Once);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }

    [Theory]
    [ClassData(typeof(UpdateCategoryCommandWhenCategoryNameIsValidAndNotProvideParentIdData))]
    public async Task HandleUpdateCategoryCommand_WhenCategoryNameIsValidAndNotProvideParentId_ShouldUpdateCategory(UpdateCategoryCommand command)
    {
        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Updated);

        _mockCategoryRepository.Verify(m => m.GetByIdAsync(command.Id), Times.Once);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task HandleUpdateCategoryCommand_WhenCategoryIsNotFound_ShouldReturnCategoryNotFoundError()
    {
        var categoryId = CategoryId.CreateUnique();
        var command = UpdateCategoryCommandUtils.CreateCommand(categoryId);
        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Category.NotFound);

        _mockCategoryRepository.Verify(m => m.GetByIdAsync(categoryId), Times.Once);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task HandleUpdateCategoryCommand_WhenCategoryIsFoundAndCategoryParentIsNotFound_ShouldReturnParentCategoryNotFoundError()
    {
        var parentCategoryId = CategoryId.CreateUnique();
        var command = UpdateCategoryCommandUtils.CreateCommand(
            Constants.Category.Id,
            Constants.Category.Name,
            parentCategoryId);

        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Category.ParentCategoryNotFound);

        _mockCategoryRepository.Verify(m => m.GetByIdAsync(Constants.Category.Id), Times.Once);
        _mockCategoryRepository.Verify(m => m.GetByIdAsync(parentCategoryId), Times.Once);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(default), Times.Never);
    }

    [Theory]
    [ClassData(typeof(UpdateCategoryCommandWhenCategoryNameIsInvalidData))]
    public async Task HandleUpdateCategoryCommand_WhenCategoryNameIsInvalid_ShouldReturnInvalidNameLengthError(UpdateCategoryCommand command)
    {
        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Category.InvalidNameLength);

        _mockCategoryRepository.Verify(m => m.GetByIdAsync(command.Id), Times.Once);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(default), Times.Never);
    }
}

public class UpdateCategoryCommandWhenCategoryNameAndParentIdIsValidData : TheoryData<UpdateCategoryCommand>
{
    public UpdateCategoryCommandWhenCategoryNameAndParentIdIsValidData()
    {
        Add(UpdateCategoryCommandUtils.CreateCommand(
            Constants.Category.Id,
            Constants.Category.Name,
            Constants.Category.ParentId));

        Add(UpdateCategoryCommandUtils.CreateCommand(
            Constants.Category.Id,
            Constants.Category.NameHasMinLength,
            Constants.Category.ParentId));

        Add(UpdateCategoryCommandUtils.CreateCommand(
            Constants.Category.Id,
            Constants.Category.NameHasMaxLength,
            Constants.Category.ParentId));
    }
}

public class UpdateCategoryCommandWhenCategoryNameIsValidAndNotProvideParentIdData : TheoryData<UpdateCategoryCommand>
{
    public UpdateCategoryCommandWhenCategoryNameIsValidAndNotProvideParentIdData()
    {
        Add(UpdateCategoryCommandUtils.CreateCommand(
            Constants.Category.Id,
            Constants.Category.Name));

        Add(UpdateCategoryCommandUtils.CreateCommand(
            Constants.Category.Id,
            Constants.Category.NameHasMinLength));

        Add(UpdateCategoryCommandUtils.CreateCommand(
            Constants.Category.Id,
            Constants.Category.NameHasMaxLength));
    }
}

public class UpdateCategoryCommandWhenCategoryNameIsInvalidData : TheoryData<UpdateCategoryCommand>
{
    public UpdateCategoryCommandWhenCategoryNameIsInvalidData()
    {
        Add(UpdateCategoryCommandUtils.CreateCommand(
            Constants.Category.Id,
            Constants.Category.NameUnderMinLength));
            
        Add(UpdateCategoryCommandUtils.CreateCommand(
            Constants.Category.Id,
            Constants.Category.NameExceedMaxLength));
    }
}

