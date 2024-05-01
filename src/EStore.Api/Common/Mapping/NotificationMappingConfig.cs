using EStore.Application.Notifications.Queries.GetNotificationsByCustomerId;
using EStore.Contracts.Notifications;
using EStore.Domain.CustomerAggregate.ValueObjects;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class NotificationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<GetNotificationsByCustomerRequest, GetNotificationsByCustomerIdQuery>()
            .Map(dest => dest.CustomerId, src => CustomerId.Create(src.CustomerId));
    }
}
