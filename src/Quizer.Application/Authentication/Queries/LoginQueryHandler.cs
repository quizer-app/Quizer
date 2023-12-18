using MediatR;
using Quizer.Application.Authentication.Common;
using Quizer.Application.Common.Exceptions;
using Quizer.Application.Common.Interfaces.Authentication;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Entities;

namespace Quizer.Application.Authentication.Queries
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<AuthenticationResult> Handle(LoginQuery query, CancellationToken cancellation)
        {
            if (await _userRepository.GetUserByEmail(query.Email) is not User user || user.Password != query.Password)
            {
                throw new BadRequestException("Invalid email or password.");
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                user,
                token);
        }
    }
}
