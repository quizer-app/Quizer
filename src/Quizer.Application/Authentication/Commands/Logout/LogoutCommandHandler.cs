using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Quizer.Application.Common.Interfaces.Authentication;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.RefreshTokenAggregate;
using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Authentication.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ErrorOr<LogoutResult>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly SignInManager<User> _signInManager;

    public LogoutCommandHandler(SignInManager<User> signInManager, IRefreshTokenRepository refreshTokenRepository)
    {
        _signInManager = signInManager;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<ErrorOr<LogoutResult>> Handle(LogoutCommand query, CancellationToken cancellation)
    {
        var token = await _refreshTokenRepository.Get(TokenId.Create(query.RefreshToken));

        if(token is null)
            return Errors.Authentication.InvalidCredentials;

        _refreshTokenRepository.Delete(token);
        await _signInManager.SignOutAsync();

        return new LogoutResult(true);
    }
}
