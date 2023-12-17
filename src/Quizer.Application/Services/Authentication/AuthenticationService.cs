using Quizer.Application.Common.Interfaces.Authentication;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Entities;

namespace Quizer.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
        {
            if(await _userRepository.GetUserByEmail(email) is not null)
                throw new Exception("User with this email already exists.");

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };
            await _userRepository.Add(user);

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                user,
                token);
        }

        public async Task<AuthenticationResult> Login(string email, string password)
        {
            if (await _userRepository.GetUserByEmail(email) is not User user)
            {
                throw new Exception("User with this email does not exist.");
            }

            if (user.Password != password)
            {
                throw new Exception("Invalid password.");
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                user,
                token);
        }
    }
}
