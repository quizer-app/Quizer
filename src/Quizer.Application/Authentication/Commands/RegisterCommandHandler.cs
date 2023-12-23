using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Authentication.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<User>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterCommandHandler> _logger;

        public RegisterCommandHandler(UserManager<User> userManager, ILogger<RegisterCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<ErrorOr<User>> Handle(RegisterCommand command, CancellationToken cancellationToken)
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

            return user;
        }
    }
}
