using EStore.Application.Customers.Command.UpdateCustomer;
using EStore.Application.Customers.Commands.AddAddress;
using EStore.Application.Customers.Commands.UpdateAddress;
using EStore.Contracts.Common;
using EStore.Contracts.Customers;
using EStore.Domain.CustomerAggregate.Entities;
using EStore.Domain.CustomerAggregate.ValueObjects;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class CustomerMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Guid, UpdateCustomerRequest), UpdateCustomerCommand>()
            .Map(dest => dest.Id, src => CustomerId.Create(src.Item1))
            .Map(dest => dest, src => src.Item2);

        config.NewConfig<(Guid, AddAddressRequest), AddAddressCommand>()
            .Map(dest => dest.CustomerId, src => CustomerId.Create(src.Item1))
            .Map(dest => dest, src => src.Item2);

        config.NewConfig<(Guid, Guid, UpdateAddressRequest), UpdateAddressCommand>()
            .Map(dest => dest.CustomerId, src => CustomerId.Create(src.Item1))
            .Map(dest => dest.AddressId, src => AddressId.Create(src.Item2))
            .Map(dest => dest, src => src.Item3);

        config.NewConfig<Address, AddressResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest, src => src);
    }
}
