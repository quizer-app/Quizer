using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Quizer.Application.Common.Interfaces.Authentication;
using Quizer.Domain.Common.Errors;

namespace Quizer.Application.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<string>>
{
    private readonly ILogger<RefreshTokenCommandHandler> _logger;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RefreshTokenCommandHandler(ILogger<RefreshTokenCommandHandler> logger, IJwtTokenGenerator jwtTokenGenerator)
    {
        _logger = logger;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ErrorOr<string>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        bool isValid = await _jwtTokenGenerator.ValidateRefreshToken(command.RefreshToken);
        if (!isValid)
        {
            _logger.LogWarning("Invalid refresh token {refreshToken}", command.RefreshToken);
            return Errors.Authentication.InvalidToken;
        }

        string accessToken = _jwtTokenGenerator.GenerateAccessToken(command.RefreshToken);
        _logger.LogInformation("Generated access token {accessToken}", accessToken);

        return accessToken;
    }
}
