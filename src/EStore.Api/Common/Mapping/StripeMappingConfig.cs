using EStore.Application.Orders.Commands.CreateCheckoutSession;
using EStore.Contracts.Carts;
using EStore.Domain.CustomerAggregate.ValueObjects;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class StripeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Guid, CheckoutRequest), CreateCheckoutSessionCommand>()
            .Map(dest => dest.CustomerId, src => CustomerId.Create(src.Item1))
            .Map(dest => dest.AddressId, src => src.Item2.AddressId)
            .Map(dest => dest, src => src.Item2);
    }
}
