using ErrorOr;
using EStore.Domain.OrderAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Orders.Commands.ConfirmPaymentInfo;

public record ConfirmPaymentInfoCommand(OrderId OrderId)
    : IRequest<ErrorOr<Success>>;
