using EStore.Domain.Common.Abstractions;
using EStore.Domain.CustomerAggregate.Events;
using EStore.Domain.CustomerAggregate.ValueObjects;
using Newtonsoft.Json;

namespace EStore.Application.Customers.Events.CustomerCreated;

public class CustomerCreatedIntegrationEvent : IIntegrationEvent
{
    public CustomerId CustomerId { get; }

    public string Email { get; }

    internal CustomerCreatedIntegrationEvent(CustomerCreatedDomainEvent customerCreatedDomainEvent)
    {
        CustomerId = customerCreatedDomainEvent.CustomerId;
        Email = customerCreatedDomainEvent.Email;
    }

    [JsonConstructor]
    private CustomerCreatedIntegrationEvent(Guid customerId, string email)
    {
        CustomerId = CustomerId.Create(customerId);
        Email = email;
    }
}
