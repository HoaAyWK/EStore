using EStore.Application.Notifications.Command.MarkAllNotificationsAsRead;
using EStore.Application.Notifications.Command.MarkNotificationAsRead;
using EStore.Application.Notifications.Queries.GetNotificationById;
using EStore.Application.Notifications.Queries.GetNotificationsByCustomerId;
using EStore.Contracts.Notifications;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.NotificationAggregate.ValueObjects;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class NotificationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<GetNotificationsByCustomerRequest, GetNotificationsByCustomerIdQuery>()
            .Map(dest => dest.CustomerId, src => CustomerId.Create(src.CustomerId));

        config.NewConfig<Guid, GetNotificationsByCustomerIdQuery>()
            .Map(dest => dest.CustomerId, src => CustomerId.Create(src));

        config.NewConfig<Guid, GetNotificationsByCustomerIdQuery>()
            .Map(dest => dest.CustomerId, src => CustomerId.Create(src));

        config.NewConfig<Guid, MarkAllNotificationsAsReadCommand>()
            .Map(dest => dest.CustomerId, src => CustomerId.Create(src));

        config.NewConfig<Guid, MarkNotificationAsReadCommand>()
            .Map(dest => dest.NotificationId, src => NotificationId.Create(src));

        config.NewConfig<Guid, GetNotificationByIdQuery>()
            .Map(dest => dest.Id, src => NotificationId.Create(src));
    }
}
