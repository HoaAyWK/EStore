using EStore.Application.Common.Dtos;
using EStore.Contracts.Authentication;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest.Id, src => src.Customer.Id.Value)
            .Map(dest => dest, src => src.Customer);
    }
}
