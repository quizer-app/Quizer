using ErrorOr;
using MediatR;
using Quizer.Application.Authentication.Common;
using Quizer.Application.Common.Interfaces.Authentication;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Authentication.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellation)
        {
            //if (await _userRepository.GetUserByEmail(command.Email) is not null)
            //    return Errors.User.DuplicateEmail;

            //var user = new User
            //{
            //    FirstName = command.FirstName,
            //    LastName = command.LastName,
            //    Email = command.Email,
            //    Password = command.Password
            //};
            //await _userRepository.Add(user);

            //var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                null,
                string.Empty);
        }
    }
}
