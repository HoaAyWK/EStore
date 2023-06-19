using EStore.Application.Brands.Commands.CreateBrand;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Application.UnitTests.Brands.Commands.TestUtils;
using EStore.Application.UnitTests.Brands.Commands.TestUtils.Brands.Extensions;
using EStore.Application.UnitTests.TestUtils.Constants;
using EStore.Domain.BrandAggregate.Repositories;
using EStore.Domain.Common.Errors;
using Moq;


namespace EStore.Application.UnitTests.Brands.Commands.CreateBrand;

public class CreateBrandCommandHandlerTests
{
    private readonly CreateBrandCommandHandler _handler;
    private readonly Mock<IBrandRepository> _mockBrandRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public CreateBrandCommandHandlerTests()
    {
        _mockBrandRepository = new();
        _mockUnitOfWork = new();
        _handler = new CreateBrandCommandHandler(
            _mockBrandRepository.Object,
            _mockUnitOfWork.Object);
    }

    [Theory]
    [ClassData(typeof(CreateBrandCommandWhenBrandIsValidData))]
    public async Task HandleCreateBrandCommand_WhenBrandIsValid_ShouldCreateAndReturnBrand(CreateBrandCommand command)
    {
        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeFalse();
        result.Value.ValidateCreatedFrom(command);
        _mockBrandRepository.Verify(m => m.AddAsync(result.Value), Times.Once);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }

    [Theory]
    [ClassData(typeof(CreateBrandCommandWhenBrandNameIsNotValidData))]
    public async Task HandleCreateBrandCommand_WhenBrandNameIsNotValid_ShouldReturnInvalidNameLengthError(CreateBrandCommand command)
    {
        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Brand.InvalidNameLength);
        _mockBrandRepository.Verify(m => m.AddAsync(result.Value), Times.Never);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(default), Times.Never);
    }
}

public class CreateBrandCommandWhenBrandIsValidData : TheoryData<CreateBrandCommand>
{
    public CreateBrandCommandWhenBrandIsValidData()
    {
        Add(CreateBrandCommandUtils.CreateCommand());
        Add(CreateBrandCommandUtils.CreateCommand(Constants.Brand.NameHasMinLength));
        Add(CreateBrandCommandUtils.CreateCommand(Constants.Brand.NameHasMaxLength));
    }
}

public class CreateBrandCommandWhenBrandNameIsNotValidData : TheoryData<CreateBrandCommand>
{
    public CreateBrandCommandWhenBrandNameIsNotValidData()
    {
        Add(CreateBrandCommandUtils.CreateCommand(Constants.Brand.NameUnderMinLength));
        Add(CreateBrandCommandUtils.CreateCommand(Constants.Brand.NameExceedMaxLength));
    }
}
