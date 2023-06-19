using EStore.Application.Common.Interfaces.Persistence;
using EStore.Application.Products.Commands.CreateProduct;
using EStore.Application.UnitTests.Products.Commands.TestUtils;
using EStore.Application.UnitTests.Products.Commands.TestUtils.Products.Extensions;
using EStore.Application.UnitTests.TestUtils.Constants;
using EStore.Domain.BrandAggregate.Repositories;
using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.CategoryAggregate.Repositories;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.Common.Errors;
using EStore.Domain.ProductAggregate.Repositories;
using Moq;

namespace EStore.Application.UnitTests.Products.Commands.CreateProduct;

public class CreateProductCommandHandlerTests
{
    private readonly CreateProductCommandHandler _handler;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IBrandRepository> _mockBrandRepository;
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public CreateProductCommandHandlerTests()
    {
        _mockProductRepository = new();
        _mockUnitOfWork = new();
        _mockBrandRepository = new();
        _mockCategoryRepository = new();

        _handler = new(
            _mockProductRepository.Object,
            _mockBrandRepository.Object,
            _mockCategoryRepository.Object,
            _mockUnitOfWork.Object);

        _mockCategoryRepository.Setup(m => m.GetByIdAsync(Constants.Product.CategoryId))
            .ReturnsAsync(Constants.Category.MockExistingCategoryNoParent);

        _mockBrandRepository.Setup(m => m.GetByIdAsync(Constants.Product.BrandId))
            .ReturnsAsync(Constants.Brand.MockExistingBrand);
    }

    [Theory]
    [ClassData(typeof(CreateProductCommandWhenProductIsValidData))]
    public async Task HandleCreateProductCommand_WhenProductIsValid_ShouldCreateAndReturnProduct(CreateProductCommand command)
    {
        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeFalse();
        result.Value.ValidateCreatedFrom(command);

        _mockProductRepository.Verify(m => m.AddAsync(result.Value), Times.Once);
        _mockBrandRepository.Verify(m => m.GetByIdAsync(command.BrandId), Times.Once);
        _mockCategoryRepository.Verify(m => m.GetByIdAsync(command.CategoryId), Times.Once);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }

    [Theory]
    [ClassData(typeof(CreateProductCommandWhenProductNameIsNotValidData))]
    public async Task HandleCreateProductCommand_WhenProductNameIsNotValid_ShouldReturnInvalidProductNameError(CreateProductCommand command)
    {
        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Product.InvalidNameLength);

        _mockProductRepository.Verify(m => m.AddAsync(result.Value), Times.Never);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task HandleCreateProductCommand_WhenBrandIsNotFound_ShouldReturnBrandNotFoundError()
    {
        var brandId = BrandId.CreateUnique();
        var command = CreateProductCommandUtils.CreateCommand(brandId: brandId);

        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Product.BrandNotFound(brandId));

        _mockBrandRepository.Verify(m => m.GetByIdAsync(command.BrandId), Times.Once);
        _mockProductRepository.Verify(m => m.AddAsync(result.Value), Times.Never);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task HandleCreateProductCommand_WhenCategoryIsNotFound_ShouldReturnCategoryNotFoundError()
    {
        var categoryId = CategoryId.CreateUnique();
        var command = CreateProductCommandUtils.CreateCommand(categoryId: categoryId);

        var result = await _handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Product.CategoryNotFound(categoryId));

        _mockCategoryRepository.Verify(m => m.GetByIdAsync(command.CategoryId), Times.Once);
        _mockProductRepository.Verify(m => m.AddAsync(result.Value), Times.Never);
        _mockUnitOfWork.Verify(m => m.SaveChangesAsync(default), Times.Never);
    }
}

public class CreateProductCommandWhenProductIsValidData : TheoryData<CreateProductCommand>
{
    public CreateProductCommandWhenProductIsValidData()
    {
        Add(CreateProductCommandUtils.CreateCommand());
        Add(CreateProductCommandUtils.CreateCommand(Constants.Product.NameHasMinLength));
        Add(CreateProductCommandUtils.CreateCommand(Constants.Product.NameHasMaxLength));
    }
}

public class CreateProductCommandWhenProductNameIsNotValidData : TheoryData<CreateProductCommand>
{
    public CreateProductCommandWhenProductNameIsNotValidData()
    {
        Add(CreateProductCommandUtils.CreateCommand(Constants.Product.NameUnderMinLength));
        Add(CreateProductCommandUtils.CreateCommand(Constants.Product.NameExceedMaxLength));
    }
}

