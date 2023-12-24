using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizer.Application.Quizes.Commands.CreateQuiz;
using Quizer.Application.Quizes.Queries.GetQuiz;
using Quizer.Application.Quizes.Queries.GetQuizes;
using Quizer.Contracts.Quiz;
using System.Security.Claims;

namespace Quizer.Api.Controllers
{
    [Route("[controller]")]
    
    public class QuizController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public QuizController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetQuizes(string? userId = null)
        {
            var query = new GetQuizesQuery(userId);
            var result = await _mediator.Send(query);
            
            return result.Match(
                quizes => Ok(_mapper.Map<List<QuizResponse>>(quizes)),
                Problem);
        }

        [HttpGet("id/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetQuizById(string id)
        {
            var query = new GetQuizQuery(id, null);
            var result = await _mediator.Send(query);

            return result.Match(
                quizes => Ok(_mapper.Map<QuizResponse>(quizes)),
                Problem);
        }

        [HttpGet("name/{name}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetQuizByName(string name)
        {
            var query = new GetQuizQuery(null, name);
            var result = await _mediator.Send(query);

            return result.Match(
                quizes => Ok(_mapper.Map<QuizResponse>(quizes)),
                Problem);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuiz(
            CreateQuizRequest request)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var command = _mapper.Map<CreateQuizCommand>((request, userId));
            var result = await _mediator.Send(command);

            return result.Match(
                quiz => Ok(_mapper.Map<QuizResponse>(quiz)),
                Problem);
        }
    }
}
