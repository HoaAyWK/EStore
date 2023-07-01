using ErrorOr;
using MediatR;

namespace EStore.Application.Orders.Commands.MarkOrderAsRefunded;

public record MarkOrderAsRefundedCommand(string TransactionId)
    : IRequest<ErrorOr<Updated>>;
