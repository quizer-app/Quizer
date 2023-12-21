using ErrorOr;
using MediatR;
using Quizer.Application.Authentication.Common;
using Quizer.Application.Common.Interfaces.Authentication;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Authentication.Queries
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellation)
        {
            //if (await _userRepository.GetUserByEmail(query.Email) is not User user)
            //{
            //    return Errors.Authentication.InvalidCredentials;
            //}

            //if(user.Password != query.Password)
            //{
            //    return Errors.Authentication.InvalidCredentials;
            //}

            //var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                null,
                string.Empty);
        }
    }
}
