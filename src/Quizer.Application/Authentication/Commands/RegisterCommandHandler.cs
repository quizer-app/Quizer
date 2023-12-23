using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Quizer.Application.Authentication.Common;
using Quizer.Application.Common.Interfaces.Authentication;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Authentication.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterCommandHandler> _logger;

        public RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator, UserManager<User> userManager, ILogger<RegisterCommandHandler> logger)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellation)
        {
            if ((await _userManager.FindByEmailAsync(command.Email)) is not null)
                return Errors.User.DuplicateEmail;
            if ((await _userManager.FindByNameAsync(command.Username)) is not null)
                return Errors.User.DuplicateUsername;

            var user = new User
            {
                UserName = command.Username,
                Email = command.Email,
            };
            var result = await _userManager.CreateAsync(user, command.Password);

            _logger.LogInformation("Registered: {@Result}", result);
            //var user = new User
            //{
            //    FirstName = command.FirstName,
            //    LastName = command.LastName,
            //    Email = command.Email,
            //    Password = command.Password
            //};
            //await _userRepository.Add(user);

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                user,
                token);
        }
    }
}
