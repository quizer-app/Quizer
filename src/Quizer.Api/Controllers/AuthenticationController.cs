using MediatR;
using Microsoft.AspNetCore.Mvc;
using Quizer.Application.Services.Authentication.Commands;
using Quizer.Application.Services.Authentication.Queries;
using Quizer.Contracts.Authentication;

namespace Quizer.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = new RegisterCommand(
                );
            var authResult = await _authenticationCommandService.Register(request.FirstName, request.LastName, request.Email, request.Password);

            var response = new AuthenticationResponse(
                authResult.User.Id,
                authResult.User.FirstName,
                authResult.User.LastName,
                authResult.User.Email,
                authResult.Token
            );

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var authResult = await _authenticationQueryService.Login(request.Email, request.Password);

            var response = new AuthenticationResponse(
                authResult.User.Id,
                authResult.User.FirstName,
                authResult.User.LastName,
                authResult.User.Email,
                authResult.Token
            );

            return Ok(response);
        }
    }
}
