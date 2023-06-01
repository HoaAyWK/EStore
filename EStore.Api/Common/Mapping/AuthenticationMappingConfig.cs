using EStore.Application.Authentication.Commands.Register;
using EStore.Application.Authentication.Common;
using EStore.Application.Authentication.Queries.Login;
using EStore.Contracts.Authentication;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();

        config.NewConfig<LoginRequest, LoginQuery>();
        
        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest, src => src.User);
    }
}
