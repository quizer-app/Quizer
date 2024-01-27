using Mapster;
using Quizer.Application.Authentication.Commands.Register;
using Quizer.Application.Authentication.Queries.Login;
using Quizer.Contracts.Authentication;

namespace Quizer.Api.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();

        config.NewConfig<LoginRequest, LoginQuery>();

        config.NewConfig<LoginResult, LoginResponse>()
            .Map(dest => dest, src => src.User);

        config.NewConfig<RegisterResult, RegisterResponse>()
            .Map(dest => dest, src => src.User);
    }
}
