using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Quizer.Application.Common.Interfaces.Authentication;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<LoginResult>>
{
    private readonly IJwtTokenProvider _jwtTokenGenerator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public LoginCommandHandler(IJwtTokenProvider jwtTokenGenerator, UserManager<User> userManager, SignInManager<User> signInManager, IRefreshTokenRepository refreshTokenRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userManager = userManager;
        _signInManager = signInManager;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<ErrorOr<LoginResult>> Handle(LoginCommand query, CancellationToken cancellation)
    {
        var user = await _userManager.FindByEmailAsync(query.Email);
        if (user is null)
            return Errors.Authentication.InvalidCredentials;

        var result = await _signInManager.PasswordSignInAsync(user, query.Password, isPersistent: false, lockoutOnFailure: false);

        if (!result.Succeeded)
            return Errors.Authentication.InvalidCredentials;

        string accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
        string refreshToken = _jwtTokenGenerator.GenerateRefreshToken(user);

        var token = Domain.RefreshTokenAggregate.RefreshToken.Create(refreshToken);
        await _refreshTokenRepository.Add(token);

        return new LoginResult(
            user,
            accessToken,
            refreshToken);
    }
}
