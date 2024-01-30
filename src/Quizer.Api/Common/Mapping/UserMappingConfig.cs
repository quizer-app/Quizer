using Mapster;
using Quizer.Application.Users.Common;
using Quizer.Contracts.User;

namespace Quizer.Api.Common.Mapping;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserResult, UserResponse>()
            .Map(dest => dest.UserId, src => src.UserId.ToString())
            .Map(dest => dest, src => src);
    }
}
