using EStore.Application.Customers.Commands.CreateCustomer;
using EStore.Contracts.Accounts;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class AcctounMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateUserRequest, CreateCustomerCommand>();
    }
}