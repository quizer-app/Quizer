using Mapster;
using Quizer.Application.Authentication.Commands;
using Quizer.Application.Authentication.Common;
using Quizer.Application.Authentication.Queries;
using Quizer.Contracts.Authentication;

namespace Quizer.Api.Common.Mapping
{
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
}
