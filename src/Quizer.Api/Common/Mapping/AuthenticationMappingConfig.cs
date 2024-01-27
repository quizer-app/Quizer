using Mapster;
using Quizer.Application.Authentication.Commands.Login;
using Quizer.Application.Authentication.Commands.Register;
using Quizer.Contracts.Authentication;

namespace Quizer.Api.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();

        config.NewConfig<LoginRequest, LoginCommand>();

        config.NewConfig<LoginResult, LoginResponse>()
            .Map(dest => dest, src => src.User);

        config.NewConfig<RegisterResult, RegisterResponse>()
            .Map(dest => dest, src => src.User);
    }
}
