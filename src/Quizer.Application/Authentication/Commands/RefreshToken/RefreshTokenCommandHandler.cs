using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Quizer.Application.Common.Interfaces.Authentication;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.RefreshTokenAggregate;

namespace Quizer.Application.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<string>>
{
    private readonly ILogger<RefreshTokenCommandHandler> _logger;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RefreshTokenCommandHandler(ILogger<RefreshTokenCommandHandler> logger, IJwtTokenGenerator jwtTokenGenerator, IRefreshTokenRepository refreshTokenRepository)
    {
        _logger = logger;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<ErrorOr<string>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        bool isValid = await _jwtTokenGenerator.ValidateRefreshToken(command.RefreshToken);
        if (!isValid)
        {
            _logger.LogError("Invalid refresh token {refreshToken}", command.RefreshToken);
            return Errors.Authentication.InvalidToken;
        }

        var token = await _refreshTokenRepository.Get(TokenId.Create(command.RefreshToken));
        if (token is null || !token.IsValid)
        {
            _logger.LogError("Refresh token {refreshToken} not found or not valid", command.RefreshToken);
            return Errors.Authentication.InvalidToken;
        }

        string accessToken = _jwtTokenGenerator.GenerateAccessToken(command.RefreshToken);
        _logger.LogInformation("Generated access token {accessToken}", accessToken);

        return accessToken;
    }
}
