using ErrorOr;
using EStore.Application.Common.Searching;
using EStore.Contracts.Searching;
using MediatR;

namespace EStore.Application.Searching.Commands.RebuildProductVariant;

public class RebuildProductVariantCommandHandler
    : IRequestHandler<RebuildProductVariantCommand, ErrorOr<RebuildResult>>
{
    private readonly ISearchProductsService _searchProductsService;

    public RebuildProductVariantCommandHandler(
        ISearchProductsService searchProductsService)
    {
        _searchProductsService = searchProductsService;
    }

    public async Task<ErrorOr<RebuildResult>> Handle(
        RebuildProductVariantCommand request,
        CancellationToken cancellationToken)
    {
        return await _searchProductsService.RebuildProductAsync(
            request.ProductId,
            request.ProductVariantId);
    }
}
