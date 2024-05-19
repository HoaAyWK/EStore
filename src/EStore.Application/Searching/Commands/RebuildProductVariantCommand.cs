using ErrorOr;
using EStore.Contracts.Searching;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Searching.Commands;

public record RebuildProductVariantCommand(
    ProductId ProductId,
    ProductVariantId ProductVariantId)
    : IRequest<ErrorOr<RebuildResult>>;
