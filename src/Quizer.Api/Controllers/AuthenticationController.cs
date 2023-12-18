using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Quizer.Application.Authentication.Commands;
using Quizer.Application.Authentication.Queries;
using Quizer.Contracts.Authentication;

namespace Quizer.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthenticationController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = _mapper.Map<RegisterCommand>(request);

            var authResult = await _mediator.Send(command);

            var response = _mapper.Map<AuthenticationResponse>(authResult);

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var query = _mapper.Map<LoginQuery>(request);
            var authResult = await _mediator.Send(query);

            var response = _mapper.Map<AuthenticationResponse>(authResult);

            return Ok(response);
        }
    }
}
