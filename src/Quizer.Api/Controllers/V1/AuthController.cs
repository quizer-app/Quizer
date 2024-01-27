using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Quizer.Application.Authentication.Commands.Register;
using Quizer.Application.Authentication.Queries.Login;
using Quizer.Contracts.Authentication;
using Quizer.Infrastructure.Authentication;

namespace Quizer.Api.Controllers.V1;

[Route("api/v{version:apiVersion}/[controller]")]
[AllowAnonymous]
public class AuthController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    private readonly IOptions<JwtSettings> _jwtSettings;

    public AuthController(ISender mediator, IMapper mapper, IOptions<JwtSettings> jwtSettings)
    {
        _mediator = mediator;
        _mapper = mapper;
        _jwtSettings = jwtSettings;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);

        var authResult = await _mediator.Send(command);

        return authResult.Match(
            authResult => Ok(_mapper.Map<RegisterResponse>(authResult)),
            Problem
            );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = _mapper.Map<LoginQuery>(request);

        var authResult = await _mediator.Send(query);

        DateTimeOffset? expirationTime = request.RememberMe ? DateTimeOffset.UtcNow.AddDays(_jwtSettings.Value.RefreshTokenExpiryDays) : null;
        if (!authResult.IsError)
            Response.Cookies.Append("refreshToken", authResult.Value.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expirationTime
            });

        return authResult.Match(
            authResult => Ok(_mapper.Map<LoginResponse>(authResult)),
            Problem
            );
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        string? refreshToken = Request.Cookies["refreshToken"];
        if (refreshToken is null)
            return Unauthorized();

        var query = new RefreshTokenCommand(refreshToken);

        var authResult = await _mediator.Send(query);

        return authResult.Match(
            authResult => Ok(_mapper.Map<RefreshTokenResponse>(authResult)),
            Problem
            );
    }
}
