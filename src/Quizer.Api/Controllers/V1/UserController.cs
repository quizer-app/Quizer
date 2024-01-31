using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizer.Application.Users.Queries.GetUser;
using Quizer.Application.Users.Queries.GetUserById;
using Quizer.Contracts.User;

namespace Quizer.Api.Controllers.V1;

[Route("api/v{version:apiVersion}/[controller]")]

public class UserController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public UserController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet("{userName}")]
    public async Task<IActionResult> GetUser(string userName)
    {
        var query = new GetUserQuery(userName);

        var result = await _mediator.Send(query);

        return result.Match(
            quizes => Ok(_mapper.Map<UserResponse>(quizes)),
            Problem);
    }

    [AllowAnonymous]
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        var query = new GetUserByIdQuery(userId);

        var result = await _mediator.Send(query);

        return result.Match(
            quizes => Ok(_mapper.Map<UserResponse>(quizes)),
            Problem);
    }
}
