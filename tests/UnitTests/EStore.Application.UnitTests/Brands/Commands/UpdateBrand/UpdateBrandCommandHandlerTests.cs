using EStore.Application.Brands.Commands.UpdateBrand;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.BrandAggregate.Repositories;
using EStore.Application.UnitTests.TestUtils.Constants;
using EStore.Application.UnitTests.Brands.Commands.TestUtils;
using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.Common.Errors;
using ErrorOr;
using Moq;

namespace EStore.Application.UnitTests.Brands.Commands.UpdateBrand;

public class UpdateBrandCommandHandlerTests
{
    private readonly UpdateBrandCommandHandler _handler;
    private readonly Mock<IBrandRepository> _mockBrandRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public UpdateBrandCommandHandlerTests()
    {
        _mockBrandRepository = new();
        _mockUnitOfWork = new();

        _mockBrandRepository.Setup(m => m.GetByIdAsync(Constants.Brand.MockExistingBrand.Id))
            .ReturnsAsync(Constants.Brand.MockExistingBrand);

        _handler = new UpdateBrandCommandHandler(
            _mockBrandRepository.Object,
            _mockUnitOfWork.Object);
    }

    [Theory]
    [ClassData(typeof(UpdateBrandCommandWhenBrandIsFoundAndNameIsValidData))]
    public async Task HandleUpdateBrandCommand_WhenBrandIsFoundAndNameIsValid_ShouldUpdateBrand(UpdateBrandCommand command)
    {
        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Updated);

        _mockBrandRepository.Verify(m => m.GetByIdAsync(command.Id), Times.Once);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }

    [Theory]
    [ClassData(typeof(UpdateBrandCommandWhenBrandIsFoundAndNameIsNotValidData))]
    public async Task HandleUpdateBrandCommand_WhenBrandIsFoundAndNameIsNotValid_ShouldReturnInvalidNameLengthError(UpdateBrandCommand command)
    {
        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Brand.InvalidNameLength);

        _mockBrandRepository.Verify(m => m.GetByIdAsync(command.Id), Times.Once);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task HandleUpdateBrandCommand_WhenBrandIsNotFound_ShouldReturnBrandNotFoundError()
    {
        var command = UpdateBrandCommandUtils.CreateCommand(BrandId.CreateUnique());

        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Brand.NotFound);

        _mockBrandRepository.Verify(m => m.GetByIdAsync(command.Id), Times.Once);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(default), Times.Never);
    }
}

public class UpdateBrandCommandWhenBrandIsFoundAndNameIsValidData : TheoryData<UpdateBrandCommand>
{
    public UpdateBrandCommandWhenBrandIsFoundAndNameIsValidData()
    {
        Add(UpdateBrandCommandUtils.CreateCommand(Constants.Brand.MockExistingBrand.Id));
        Add(UpdateBrandCommandUtils.CreateCommand(Constants.Brand.MockExistingBrand.Id, Constants.Brand.NameHasMinLength));
        Add(UpdateBrandCommandUtils.CreateCommand(Constants.Brand.MockExistingBrand.Id, Constants.Brand.NameHasMaxLength));
    }
}

public class UpdateBrandCommandWhenBrandIsFoundAndNameIsNotValidData : TheoryData<UpdateBrandCommand>
{
    public UpdateBrandCommandWhenBrandIsFoundAndNameIsNotValidData()
    {
        Add(UpdateBrandCommandUtils.CreateCommand(Constants.Brand.MockExistingBrand.Id, Constants.Brand.NameUnderMinLength));
        Add(UpdateBrandCommandUtils.CreateCommand(Constants.Brand.MockExistingBrand.Id, Constants.Brand.NameExceedMaxLength));
    }
}

