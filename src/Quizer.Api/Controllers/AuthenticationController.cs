using Microsoft.AspNetCore.Mvc;
using Quizer.Application.Services.Authentication;
using Quizer.Contracts.Authentication;

namespace Quizer.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationCommandService _authenticationService;

        public AuthenticationController(IAuthenticationCommandService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var authResult = await _authenticationService.Register(request.FirstName, request.LastName, request.Email, request.Password);

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
            var authResult = await _authenticationService.Login(request.Email, request.Password);

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
