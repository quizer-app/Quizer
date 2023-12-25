using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Quizer.Application.Common.Interfaces.Authentication;
using Quizer.Domain.Common.Errors;
using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Authentication.Queries;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<LoginResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<ErrorOr<LoginResult>> Handle(LoginQuery query, CancellationToken cancellation)
    {
        var user = await _userManager.FindByEmailAsync(query.Email);
        if (user is null)
            return Errors.Authentication.InvalidCredentials;

        var result = await _signInManager.PasswordSignInAsync(user, query.Password, isPersistent: false, lockoutOnFailure: false);

        if (!result.Succeeded)
            return Errors.Authentication.InvalidCredentials;

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new LoginResult(
            user,
            token);
    }
}
