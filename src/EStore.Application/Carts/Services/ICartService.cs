using EStore.Domain.CartAggregate.ValueObjects;

namespace EStore.Application.Carts.Services;

public interface ICartService
{
    Task TransferCartAsync(Guid anonymousId, Guid customerId);
}
