using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizer.Application.Quizes.Commands.CreateQuiz;
using Quizer.Application.Quizes.Commands.DeleteQuiz;
using Quizer.Application.Quizes.Commands.UpdateQuiz;
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

        [HttpGet("id/{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetQuizById(Guid id)
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
                quizId => Ok(_mapper.Map<QuizIdResponse>(quizId)),
                Problem);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateQuiz(
            Guid id,
            [FromBody] UpdateQuizRequest request)
        {
            var command = _mapper.Map<UpdateQuizCommand>((request, id));
            var result = await _mediator.Send(command);

            return result.Match(
                quizId => Ok(_mapper.Map<QuizIdResponse>(quizId)),
                Problem);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteQuiz(Guid id)
        {
            var command = new DeleteQuizCommand(id);
            var result = await _mediator.Send(command);

            return result.Match(
                quizId => Ok(_mapper.Map<QuizIdResponse>(quizId)),
                Problem);
        }
    }
}
