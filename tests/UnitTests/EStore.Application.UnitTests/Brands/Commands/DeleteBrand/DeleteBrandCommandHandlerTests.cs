using ErrorOr;
using EStore.Application.Brands.Commands.DeleteBrand;
using EStore.Application.UnitTests.Brands.Commands.TestUtils;
using EStore.Application.UnitTests.TestUtils.Constants;
using EStore.Domain.BrandAggregate.Repositories;
using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.Common.Errors;
using EStore.Domain.ProductAggregate.Repositories;
using Moq;

namespace EStore.Application.UnitTests.Brands.Commands.DeleteBrand;

public class DeleteBrandCommandHandlerTests
{
    private readonly DeleteBrandCommandHandler _handler;
    private readonly Mock<IBrandRepository> _mockBrandRepository;
    private readonly Mock<IProductRepository> _mockProductRepository;

    public DeleteBrandCommandHandlerTests()
    {
        _mockBrandRepository = new();
        _mockProductRepository = new();

        _mockBrandRepository.Setup(m => m.GetByIdAsync(Constants.Brand.MockExistingBrand.Id))
            .ReturnsAsync(Constants.Brand.MockExistingBrand);

        _handler = new DeleteBrandCommandHandler(
            _mockBrandRepository.Object,
            _mockProductRepository.Object);
    }

    [Fact]
    public async Task HandleDeleteBrandCommand_WhenBrandIsNotFound_ShouldReturnBrandNotFoundError()
    {
        var deleteBrandCommand = DeleteBrandCommandUtils.CreateCommand(BrandId.CreateUnique());

        var result = await _handler.Handle(deleteBrandCommand, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Brand.NotFound);

        _mockBrandRepository.Verify(m => m.GetByIdAsync(deleteBrandCommand.Id), Times.Once);
        _mockProductRepository.Verify(m => m.AnyProductWithBrandId(deleteBrandCommand.Id), Times.Never);
    }

    [Fact]
    public async Task HandleDeleteBrandCommand_WhenBrandIsFoundAndBrandHasNoProduct_ShouldDeleteBrand()
    {
        var deleteBrandCommand = DeleteBrandCommandUtils.CreateCommand();

        var result = await _handler.Handle(deleteBrandCommand, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Deleted);

        _mockBrandRepository.Verify(m => m.GetByIdAsync(deleteBrandCommand.Id), Times.Once);
        _mockProductRepository.Verify(m => m.AnyProductWithBrandId(deleteBrandCommand.Id), Times.Once);
    }


    // TODO: handle brand has products
}
