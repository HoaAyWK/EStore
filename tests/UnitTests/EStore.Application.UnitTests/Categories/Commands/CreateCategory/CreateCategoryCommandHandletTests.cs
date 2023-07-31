using EStore.Application.Categories.Commands.CreateCategory;
using EStore.Application.UnitTests.Categories.TestUtils;
using EStore.Application.UnitTests.Categories.TestUtils.Categories.Extensions;
using EStore.Application.UnitTests.TestUtils.Constants;
using EStore.Domain.CategoryAggregate.Repositories;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.Common.Errors;
using Moq;

namespace EStore.Application.UnitTests.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandlerTests
{
    private readonly CreateCategoryCommandHandler _handler;
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;

    public CreateCategoryCommandHandlerTests()
    {
        _mockCategoryRepository = new();
        _handler = new(_mockCategoryRepository.Object);

        _mockCategoryRepository.Setup(m => m.GetByIdAsync(Constants.Category.ParentId))
            .ReturnsAsync(Constants.Category.MockExistingParentCategory);
    }

    [Theory]
    [ClassData(typeof(CreateCategoryCommandWhenCategoryIsValidAndDoesNotHaveParentIdData))]
    public async Task HandleCreateCategoryCommand_WhenCategoryIsValidAndDoesNotHaveParentId_ShouldCreateAndReturnCategory(CreateCategoryCommand command)
    {
        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeFalse();
        result.Value.ValidateCreatedFrom(command);

        _mockCategoryRepository.Verify(m => m.AddAsync(result.Value), Times.Once);
    }

    [Theory]
    [ClassData(typeof(CreateCategoryCommandWhenCategoryIsValidAndHasParentIdData))]
    public async Task HandleCreateCategoryCommand_WhenCategoryIsValidAndHasParentId_ShouldCreateAndReturnCategory(CreateCategoryCommand command)
    {
        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeFalse();
        result.Value.ValidateCreatedFrom(command);

        _mockCategoryRepository.Verify(m => m.GetByIdAsync(command.ParentId!), Times.Once);
        _mockCategoryRepository.Verify(m => m.AddAsync(result.Value), Times.Once);
    }

    [Fact]
    public async Task HandleCreateCategoryCommand_WhenParentCategoryIsNotFound_ShouldReturnParentCategoryNotFoundError()
    {
        var parentCategoryId = CategoryId.CreateUnique();

        var command = CreateCategoryCommandUtils.CreateCommand(
            Constants.Category.Name,
            parentCategoryId);

        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Category.ParentCategoryNotFound);

        _mockCategoryRepository.Verify(m => m.GetByIdAsync(parentCategoryId), Times.Once);
        _mockCategoryRepository.Verify(m => m.AddAsync(result.Value), Times.Never);
    }

    [Theory]
    [ClassData(typeof(CreateCategoryCommandWhenCategoryNameIsNotValidData))]
    public async Task HandleCreateCategoryCommand_WhenCategoryNameIsNotValid_ShouldReturnInvalidNameLengthError(CreateCategoryCommand command)
    {
        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Category.InvalidNameLength);

        _mockCategoryRepository.Verify(m => m.AddAsync(result.Value), Times.Never);
    }
}

public class CreateCategoryCommandWhenCategoryIsValidAndDoesNotHaveParentIdData : TheoryData<CreateCategoryCommand>
{
    public CreateCategoryCommandWhenCategoryIsValidAndDoesNotHaveParentIdData()
    {
        Add(CreateCategoryCommandUtils.CreateCommand());
        Add(CreateCategoryCommandUtils.CreateCommand(Constants.Category.NameHasMinLength));
        Add(CreateCategoryCommandUtils.CreateCommand(Constants.Category.NameHasMaxLength));    
    }
}

public class CreateCategoryCommandWhenCategoryIsValidAndHasParentIdData : TheoryData<CreateCategoryCommand>
{
    public CreateCategoryCommandWhenCategoryIsValidAndHasParentIdData()
    {
        Add(CreateCategoryCommandUtils.CreateCommand(Constants.Category.Name, Constants.Category.ParentId));    
        Add(CreateCategoryCommandUtils.CreateCommand(Constants.Category.NameHasMaxLength, Constants.Category.ParentId));    
        Add(CreateCategoryCommandUtils.CreateCommand(Constants.Category.NameHasMaxLength, Constants.Category.ParentId));    
    }
}

public class CreateCategoryCommandWhenCategoryNameIsNotValidData : TheoryData<CreateCategoryCommand>
{
    public CreateCategoryCommandWhenCategoryNameIsNotValidData()
    {
        Add(CreateCategoryCommandUtils.CreateCommand(Constants.Category.NameUnderMinLength));
        Add(CreateCategoryCommandUtils.CreateCommand(Constants.Category.NameExceedMaxLength));
        Add(CreateCategoryCommandUtils.CreateCommand(Constants.Category.NameUnderMinLength, Constants.Category.ParentId));
        Add(CreateCategoryCommandUtils.CreateCommand(Constants.Category.NameExceedMaxLength, Constants.Category.ParentId));
    }
}

