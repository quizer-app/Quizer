﻿using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Authentication.Commands.Register;

public class RefreshTokenCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<RegisterResult>>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(UserManager<User> userManager, ILogger<RefreshTokenCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<ErrorOr<RegisterResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByEmailAsync(command.Email) is not null)
            return Errors.User.DuplicateEmail;
        if (await _userManager.FindByNameAsync(command.Username) is not null)
            return Errors.User.DuplicateUsername;

        var user = new User
        {
            UserName = command.Username,
            Email = command.Email,
        };
        var result = await _userManager.CreateAsync(user, command.Password);
        if (!result.Succeeded)
            return result.Errors
                .Select(error => Error.Validation(error.Code, error.Description))
                .ToList();

        _logger.LogInformation("Registered: {@Result}", result);

        return new RegisterResult(user);
    }
}
