using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Quizer.Application.Authentication.Commands.Login;
using Quizer.Application.Authentication.Commands.RefreshToken;
using Quizer.Application.Authentication.Commands.Register;
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

        var result = await _mediator.Send(command);

        return result.Match(
            data => Ok(_mapper.Map<RegisterResponse>(data)),
            Problem
            );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = _mapper.Map<LoginCommand>(request);

        var result = await _mediator.Send(query);

        DateTimeOffset? expirationTime = request.RememberMe ? DateTimeOffset.UtcNow.AddDays(_jwtSettings.Value.RefreshTokenExpiryDays) : null;
        if (!result.IsError)
            Response.Cookies.Append("refreshToken", result.Value.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expirationTime
            });

        return result.Match(
            data => Ok(_mapper.Map<LoginResponse>(data)),
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

        var result = await _mediator.Send(query);

        return result.Match(
            token => Ok(new RefreshTokenResponse(token)),
            Problem
            );
    }
}
