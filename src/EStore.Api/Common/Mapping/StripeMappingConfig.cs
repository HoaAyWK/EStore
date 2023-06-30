using EStore.Application.Orders.Commands.CreateCheckoutSession;
using EStore.Domain.CustomerAggregate.ValueObjects;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class StripeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Guid, CreateCheckoutSessionCommand>()
            .Map(dest => dest.CustomerId, src => CustomerId.Create(src));
    }
}
